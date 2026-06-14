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

| Method | Endpoint | Description |
|---|---|---|
| `client.Orders.GetOrdersAsync(...)`             | `/orders/get`         | List orders with filters + paging + sort |
| `client.Orders.GetOrderAsync(...)`              | `/order/get`          | Get one order by id |
| `client.Orders.GetOrderItemsAsync(...)`         | `/order/items/get`    | Line items of one order |
| `client.Orders.GetMultipleOrderItemsAsync(...)` | `/orders/items/get`   | Line items for ≤50 orders |
| `client.Orders.GetDocumentAsync(...)`           | `/order/document/get` | Invoice / shipping label / carrier manifest (Base64) |
| `client.Orders.PackAsync(...)`                  | `/order/pack`         | Mark items packed |
| `client.Orders.ReadyToShipAsync(...)`           | `/order/rts`          | Mark items ready-to-ship + assign tracking |
|| `client.Orders.CancelAsync(...)`                | `/order/cancel`       | Cancel one pending item |

## Finance — typed wrappers

Access via `client.Finance`. Endpoints are added incrementally; current coverage:

| Method | Endpoint | Description |
|---|---|---|
| `client.Finance.GetPayoutStatusAsync(...)`             | `/finance/payout/status/get`                | Get payout statements filtered by creation date |
| `client.Finance.QueryAccountTransactionsAsync(...)`    | `/finance/transaction/accountTransactions/query` | Query account transactions with pagination |
| `client.Finance.QueryLogisticsFeeDetailAsync(...)`     | `/lbs/slb/queryLogisticsFeeDetail`          | Query logistics fee details |
| `client.Finance.QueryTransactionDetailsAsync(...)`     | `/finance/seller/transaction/detail`        | Query seller transaction details within a date range |

## Returns &amp; Refund — typed wrappers

Access via `client.Returns`. Endpoints are added incrementally; current coverage:

| Method | Endpoint | Description |
|---|---|---|
|| `client.Returns.GetReverseOrderDetailAsync(...)`         | `/order/reverse/return/detail/list`       | Get detailed reverse order info |
|| `client.Returns.GetReverseOrderHistoryListAsync(...)`    | `/order/reverse/return/history/list`      | Get history for a reverse order line |
|| `client.Returns.GetReverseOrderReasonListAsync(...)`     | `/order/reverse/reason/list`              | Get available reason list |
|| `client.Returns.GetReverseOrdersForSellerAsync(...)`     | `/reverse/getreverseordersforseller`      | List reverse orders with filters + paging |
|| `client.Returns.ReverseOrderCancelValidateAsync(...)`    | `/order/reverse/cancel/validate`          | Validate cancellation eligibility |
|| `client.Returns.InitReverseOrderCancelAsync(...)`        | `/order/reverse/cancel/create`            | Initiate a cancellation |
|| `client.Returns.ReverseOrderReturnUpdateAsync(...)`      | `/order/reverse/return/update`            | Accept / refund / reject a return |

## Products — typed wrappers

Access via `client.Products`. Endpoints are added incrementally; current coverage:

| Method | Endpoint | Description |
|---|---|---|
|| `client.Products.CreateProductAsync(...)`                    | `/product/create`                       | Create a new product |
|| `client.Products.UpdateProductAsync(...)`                    | `/product/update`                       | Update an existing product |
|| `client.Products.DeactivateProductAsync(...)`                | `/product/deactivate`                   | Deactivate a product |
|| `client.Products.GetProductsAsync(...)`                      | `/products/get`                         | List products with filters |
|| `client.Products.GetProductItemAsync(...)`                   | `/product/item/get`                     | Get a single product item |
|| `client.Products.AdjustSellableQuantityAsync(...)`           | `/product/stock/sellable/adjust`        | Adjust sellable stock (increment/decrement) |
|| `client.Products.UpdateSellableQuantityAsync(...)`           | `/product/stock/sellable/update`        | Update sellable stock (absolute value) |
|| `client.Products.UpdatePriceAsync(...)`                      | `/price/sellable/update`                | Update sellable price |
|| `client.Products.SetImagesAsync(...)`                        | `/images/set`                           | Set product images |
|| `client.Products.GetCategoryTreeAsync(...)`                  | `/category/tree/get`                    | Get the category tree |
|| `client.Products.GetCategoryAttributesAsync(...)`            | `/category/attributes/get`              | Get category attributes |
|| `client.Products.GetCategorySuggestionAsync(...)`            | `/category/suggestion/get`              | Suggest categories for a product name |
|| `client.Products.GetCategorySuggestionBulkAsync(...)`        | `/category/suggestion/bulk/get`         | Bulk suggest categories |
|| `client.Products.GetBrandsAsync(...)`                        | `/brands/get`                           | Get brands for a category |
|| `client.Products.BatchUpdateSizeChartAsync(...)`             | `/size/chart/batch/update`              | Batch update size charts |
|| `client.Products.GetQcStatusAsync(...)`                      | `/product/qc/status/get`                | Get QC status for a product |
|| `client.Products.GetQcAlertProductsAsync(...)`               | `/product/qc/alert/products/get`        | Get QC alert products |
|| `client.Products.GetPreQcRulesAsync(...)`                    | `/product/pre/qc/rules/get`             | Get pre-QC rules |
|| `client.Products.GetProductResponseAsync(...)`               | `/product/response/get`                 | Get product creation/update response |
|| `client.Products.GetSellerItemLimitAsync(...)`               | `/product/seller/item/limit/get`        | Get seller item limit |
|| `client.Products.GetUnfilledAttributeItemAsync(...)`         | `/product/unfilled/attribute/item/get`  | Get unfilled attribute items |
|| `client.Products.GetProductContentScoreAsync(...)`           | `/product/content/score/get`            | Get product content score |
|| `client.Products.ProductCheckAsync(...)`                     | `/product/check`                        | Check a product before creation |
|| `client.Products.QueryProductExperimentConfigAsync(...)`     | `/product/experiment/config/query`      | Query product experiment config |
|| `client.Products.ExitExperimentAsync(...)`                   | `/product/experiment/exit`              | Exit a product experiment |
|| `client.Products.GetNextCascadePropAsync(...)`               | `/product/next/cascade/prop/get`        | Get next cascade property |

## Seller — typed wrappers

Access via `client.Seller`. Endpoints are added incrementally; current coverage:

| Method | Endpoint | Description |
|---|---|---|
|| `client.Seller.GetSellerAsync(...)`                     | `/seller/get`                     | Get seller information |
|| `client.Seller.GetSellerMetricsAsync(...)`              | `/seller/metrics/get`              | Get seller metrics |
|| `client.Seller.GetSellerPerformanceAsync(...)`          | `/seller/performance/get`          | Get seller performance indicators |
|| `client.Seller.BatchQueryFollowStatusAsync(...)`        | `/shop/follow/status/batch/query`  | Batch query follow status |
|| `client.Seller.GetPickUpStoreListAsync(...)`            | `/rc/store/list/get`               | Get pickup store list |
|| `client.Seller.GetWarehouseBySellerIdAsync(...)`        | `/warehouse/seller/get`            | Get warehouse list by seller id |
|| `client.Seller.QueryWarehouseDetailAsync(...)`          | `/warehouse/detail/query`          | Query warehouse detail |
|| `client.Seller.SellerPolicyFetchAsync(...)`             | `/seller/policy/fetch`             | Fetch seller policy |
|| `client.Seller.GetSellerRegisterInfoAsync(...)`         | `/seller/register/info/get`        | Get seller register info |
|| `client.Seller.GetSubAddressAsync(...)`                 | `/seller/address/sub/get`          | Get sub-address |
|| `client.Seller.PaymentBindingAsync(...)`                | `/seller/payment/binding`          | Bind payment method |
|| `client.Seller.SellerFieldVerifyAsync(...)`             | `/seller/field/verify`             | Verify a seller field |
|| `client.Seller.GetCountryInfoAsync(...)`                | `/seller/country/info/get`         | Get country info |

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

## Per-call credentials

Two equivalent patterns. Pick whichever fits your call site.

**Inline parameter (simplest — pass on each call):**

```csharp
// Startup — no AppKey / AppSecret yet
services.AddLazClient(o => o.ServerUrl = UrlConstants.API_GATEWAY_URL_TH);

// Per request
var creds = new LazCredentials(tenant.AppKey, tenant.AppSecret);

await client.Orders.GetDocumentAsync(req, tenant.AccessToken, credentials: creds, ct);
await client.ExecuteAsync(lazReq, tenant.AccessToken, credentials: creds, ct);
await client.Auth.RefreshAccessTokenAsync(tenant.RefreshToken, credentials: creds, ct);
```

**Scoped client (reuse creds across many calls):**

```csharp
var scoped = client.WithCredentials(new LazCredentials(tenant.AppKey, tenant.AppSecret));
await scoped.Orders.GetDocumentAsync(req, tenant.AccessToken, ct);
await scoped.ExecuteAsync(lazReq, tenant.AccessToken, ct);
```

Resolution precedence on every call: **inline `credentials` param → `WithCredentials` scope → `LazClientOptions`.** If none supply both `AppKey` and `AppSecret`, the client throws `InvalidOperationException` with an actionable message.

`AppKey` / `AppSecret` on `LazClientOptions` are optional — you can register with only `ServerUrl`.

You can also override the regional gateway per request via `LazCredentials.ServerUrl`:

```csharp
var creds = new LazCredentials(
    AppKey:    tenant.LazadaAppKey,
    AppSecret: tenant.LazadaAppSecret,
    ServerUrl: UrlConstants.API_GATEWAY_URL_TH);  // optional, overrides options.ServerUrl

var scoped = client.WithCredentials(creds);

await scoped.ExecuteAsync(new LazRequest("/orders/get") { HttpMethod = Constants.METHOD_GET }, tenant.AccessToken, ct);
await scoped.Auth.RefreshAccessTokenAsync(tenant.RefreshToken, ct);
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
