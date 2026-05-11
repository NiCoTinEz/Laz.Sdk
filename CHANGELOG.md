# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

- `client.WithCredentials(LazCredentials)` — returns a scoped `ILazClient` that overrides `AppKey` / `AppSecret` (and optionally `ServerUrl`) per call without touching DI. Shares the underlying `HttpClient`. Designed for SaaS / multi-tenant scenarios where tenant credentials are loaded at request time.
- `LazCredentials` record.
- `client.Auth.CreateAccessTokenAsync(code, ct)` — typed wrapper around `/auth/token/create` on the Lazada auth gateway. Returns `LazAccessToken` with access + refresh tokens, expiry seconds, country, account info, and `country_user_info` list.
- `client.Auth.RefreshAccessTokenAsync(refreshToken, ct)` — typed wrapper around `/auth/token/refresh`.
- `LazAccessToken` + `LazCountryUserInfo` record types under `Laz.Sdk.Models`.
- `LazResponse.ReadAs<T>(JsonSerializerOptions?)` helper for deserializing the raw body of any `ExecuteAsync` call.
- Service-grouped surface: every API domain lives under a property on `ILazClient`. Currently exposes `Auth` (`IAuthService`) and `Orders` (`IOrdersService`).
- `client.Orders.GetDocumentAsync(request, accessToken, ct)` — `/order/document/get`. Returns invoice / shipping label / carrier manifest (Base64 file payload + mime type). Wraps `OrderDocumentType` enum to the wire-format string. Helper `OrderDocument.GetFileBytes()` decodes Base64 to bytes.

### Fixed

- `LazAccessToken.ExpiresIn` / `RefreshExpiresIn` now deserialize from JSON strings as Lazada actually returns them (`"expires_in":"10"`), not only from JSON numbers.

### Changed

- Auth-endpoint requests always target `UrlConstants.API_AUTHORIZATION_URL`, regardless of the regional `LazClientOptions.ServerUrl`.
- Internal `ExecuteCoreAsync` now accepts a per-call server URL so future typed wrappers can target alternative gateways.

## [0.1.0] - 2026-05-11

### Added

- Initial release.
- `ILazClient.ExecuteAsync` — async-only Lazada Open Platform REST client.
- DI integration: `AddLazClient(...)` with single-tenant and multi-tenant (named) overloads.
- `ILazClientFactory` for resolving named clients with independent `AppKey` / `AppSecret` / region.
- `LazClientOptions` with `IConfiguration` binding support.
- Six regional gateway URLs (SG, MY, VN, TH, PH, ID) plus authorization URL via `UrlConstants`.
- HMAC-SHA256 request signing via `LazUtils.SignRequest`.
- File uploads via `FileItem` and `LazRequest.FileParams`.
- Targets `net10.0`, C# 14, nullable enabled, warnings as errors.
- SourceLink-enabled symbol package (`.snupkg`).
