# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

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
