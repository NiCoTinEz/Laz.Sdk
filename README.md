# Laz.Sdk

Unofficial .NET 10 client for the [Lazada Open Platform](https://open.lazada.com/). Async-only, dependency-injection-first.

[![NuGet](https://img.shields.io/nuget/v/Laz.Sdk.svg)](https://www.nuget.org/packages/Laz.Sdk/)
[![Downloads](https://img.shields.io/nuget/dt/Laz.Sdk.svg)](https://www.nuget.org/packages/Laz.Sdk/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

> Not affiliated with Lazada or Alibaba Group. Provided as-is for developers integrating with the Lazada Open Platform REST API.

## Install

```sh
dotnet add package Laz.Sdk
```

Targets `net10.0`, C# 14. Depends on `Microsoft.Extensions.Http`.

## Quickstart — Dependency Injection

```csharp
using Laz.Sdk;
using Laz.Sdk.Util;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddLazClient(o =>
{
    o.AppKey    = "your-app-key";
    o.AppSecret = "your-app-secret";
    o.ServerUrl = UrlConstants.API_GATEWAY_URL_SG; // or TH / MY / VN / PH / ID
});

var sp = services.BuildServiceProvider();
var client = sp.GetRequiredService<ILazClient>();

var request = new LazRequest { ApiName = "/system/oauth/refresh" };
request.AddApiParameter("refresh_token", "...");

var response = await client.ExecuteAsync(request, cancellationToken: CancellationToken.None);
if (response.IsError())
{
    Console.Error.WriteLine($"{response.Code}: {response.Message}");
}
else
{
    Console.WriteLine(response.Body);
}
```

## OAuth — getting an access token

Lazada's OAuth flow redirects the seller back to your app with a one-time `code`. Exchange it for tokens:

```csharp
using Laz.Sdk;
using Laz.Sdk.Models;

var token = await client.Auth.CreateAccessTokenAsync(authorizationCode, ct);

Console.WriteLine(token.AccessToken);       // "5000..."
Console.WriteLine(token.RefreshToken);      // "5000..."
Console.WriteLine(token.ExpiresIn);         // seconds
Console.WriteLine(token.RefreshExpiresIn);  // seconds
Console.WriteLine(token.Country);           // e.g. "sg"
foreach (var info in token.CountryUserInfo ?? Array.Empty<LazCountryUserInfo>())
{
    Console.WriteLine($"{info.Country} seller_id={info.SellerId} short_code={info.ShortCode}");
}
```

Refresh before `ExpiresIn` elapses:

```csharp
var refreshed = await client.Auth.RefreshAccessTokenAsync(token.RefreshToken!, ct);
```

Both methods call the Lazada auth gateway (`https://auth.lazada.com/rest`) regardless of the regional `ServerUrl` configured in `LazClientOptions`. On error the SDK throws `LazException` with `ErrorCode` and `ErrorMsg`.

## Orders — typed wrappers

Access via `client.Orders`. Endpoints are added incrementally; current coverage:

### `client.Orders.GetDocumentAsync(...)` — `/order/document/get`

Retrieve invoice / shipping label / carrier manifest for one or more order items:

```csharp
using Laz.Sdk.Models.Orders;

var doc = await client.Orders.GetDocumentAsync(
    new GetOrderDocumentRequest
    {
        DocType      = OrderDocumentType.ShippingLabel,
        OrderItemIds = new[] { 279709L, 279710L },
    },
    accessToken: sellerAccessToken,
    cancellationToken: ct);

byte[] bytes = doc.Data!.Document!.GetFileBytes(); // decoded from Base64
File.WriteAllBytes("label.html", bytes);
```

`OrderDocumentType` values map to the wire format: `Invoice` → `"invoice"`, `ShippingLabel` → `"shippingLabel"`, `CarrierManifest` → `"carrierManifest"`. Errors surface as `LazException(ErrorCode, ErrorMsg)`.

## Per-call credential override — `client.WithCredentials(...)`

For multi-tenant apps that load credentials from a database at request time (rather than DI startup), call `WithCredentials` to get a scoped `ILazClient`:

```csharp
var tenantCreds = new LazCredentials(
    AppKey:    tenant.LazadaAppKey,
    AppSecret: tenant.LazadaAppSecret,
    ServerUrl: UrlConstants.API_GATEWAY_URL_TH);  // optional, falls back to options.ServerUrl

var scoped = client.WithCredentials(tenantCreds);

await scoped.ExecuteAsync(new LazRequest("/orders/get") { HttpMethod = Constants.METHOD_GET }, tenantAccessToken, ct);
await scoped.Orders.GetDocumentAsync(req, tenantAccessToken, ct);
await scoped.Auth.RefreshAccessTokenAsync(tenantRefreshToken, ct);
```

The original `client` is unchanged. The scoped client shares the same `HttpClient` (so resilience handlers / connection pool are reused) and overrides only signing credentials + optional gateway. Cheap to call — fine to instantiate per request.

## Multi-tenant — multiple `AppKey` / `AppSecret` known at startup

Register one named client per tenant; resolve via `ILazClientFactory`:

```csharp
services.AddLazClient("th", o =>
{
    o.AppKey    = "key-th";
    o.AppSecret = "secret-th";
    o.ServerUrl = UrlConstants.API_GATEWAY_URL_TH;
});

services.AddLazClient("id", o =>
{
    o.AppKey    = "key-id";
    o.AppSecret = "secret-id";
    o.ServerUrl = UrlConstants.API_GATEWAY_URL_ID;
});

public sealed class OrderSync(ILazClientFactory lazFactory)
{
    public async Task SyncAllAsync(CancellationToken ct)
    {
        var th = lazFactory.CreateClient("th");
        var id = lazFactory.CreateClient("id");

        await Task.WhenAll(
            th.ExecuteAsync(new LazRequest { ApiName = "/orders/get" }, cancellationToken: ct),
            id.ExecuteAsync(new LazRequest { ApiName = "/orders/get" }, cancellationToken: ct));
    }
}
```

Each named client gets its own `HttpClient` (named `Laz.Sdk:{name}`) and its own `LazClientOptions`. Each registration returns an `IHttpClientBuilder` so you can chain resilience / handlers per tenant:

```csharp
services.AddLazClient("th", configure)
        .AddStandardResilienceHandler();
```

## Binding from `IConfiguration`

```jsonc
// appsettings.json
{
  "Lazada": {
    "AppKey":    "your-app-key",
    "AppSecret": "your-app-secret",
    "ServerUrl": "https://api.lazada.sg/rest"
  }
}
```

```csharp
services.AddLazClient(configuration.GetSection("Lazada"));
```

## Configuration — `LazClientOptions`

| Property     | Default                                    | Notes                                                                  |
|--------------|--------------------------------------------|------------------------------------------------------------------------|
| `AppKey`     | *required*                                 | Lazada Open Platform app key.                                          |
| `AppSecret`  | *required*                                 | Lazada Open Platform app secret. Used for HMAC-SHA256 request signing. |
| `ServerUrl`  | `UrlConstants.API_GATEWAY_URL_SG`          | Region gateway. See **Regions** below.                                 |
| `SignMethod` | `Constants.SIGN_METHOD_SHA256` (`"sha256"`)| Only `sha256` is supported.                                            |
| `Timeout`    | 30 seconds                                 | Applied to the underlying `HttpClient.Timeout`.                        |

## Regions

| Constant                                | URL                                |
|-----------------------------------------|------------------------------------|
| `UrlConstants.API_GATEWAY_URL_SG`       | `https://api.lazada.sg/rest`       |
| `UrlConstants.API_GATEWAY_URL_MY`       | `https://api.lazada.com.my/rest`   |
| `UrlConstants.API_GATEWAY_URL_VN`       | `https://api.lazada.vn/rest`       |
| `UrlConstants.API_GATEWAY_URL_TH`       | `https://api.lazada.co.th/rest`    |
| `UrlConstants.API_GATEWAY_URL_PH`       | `https://api.lazada.com.ph/rest`   |
| `UrlConstants.API_GATEWAY_URL_ID`       | `https://api.lazada.co.id/rest`    |
| `UrlConstants.API_AUTHORIZATION_URL`    | `https://auth.lazada.com/rest`     |

## File uploads

```csharp
using Laz.Sdk.Util;

var request = new LazRequest { ApiName = "/image/upload" };
request.AddFileParameter("image", new FileItem("C:/path/to/photo.jpg"));

var response = await client.ExecuteAsync(request, cancellationToken: ct);
```

`FileItem` accepts a path, a `FileInfo`, a `byte[]`, or a `Stream`.

## Disclaimer

`Laz.Sdk` is an independent open-source project. It is **not affiliated with, endorsed by, or sponsored by** Lazada Group or Alibaba Group. "Lazada" and related marks are trademarks of their respective owners. Use of the Lazada Open Platform itself is governed by Lazada's own terms.

## License

[MIT](LICENSE)
