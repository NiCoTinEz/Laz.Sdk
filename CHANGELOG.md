# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

- Inline `credentials: LazCredentials?` parameter on `ILazClient.ExecuteAsync`, `IAuthService.CreateAccessTokenAsync` / `RefreshAccessTokenAsync`, and `IOrdersService.GetDocumentAsync`. Lets callers supply tenant credentials directly per call without `WithCredentials`. Precedence: **inline > scoped > options**.
- `client.WithCredentials(LazCredentials)` — alternative scoped form. Returns a new `ILazClient` bound to the credentials so multiple calls reuse them. Shares the underlying `HttpClient`.
- `LazCredentials` record.

### Added (Promotions API)

- `client.Promotions` — new `IPromotionsService` / `PromotionsService` group wrapping 26 endpoints:
  - **Seller Voucher (9):** `CreateVoucherAsync`, `UpdateVoucherAsync`, `GetVoucherAsync`, `GetVoucherListAsync`, `ActivateVoucherAsync`, `DeactivateVoucherAsync`, `GetVoucherProductsAsync`, `AddVoucherSkuAsync`, `RemoveVoucherSkuAsync`.
  - **Free Shipping (9):** `CreateFreeShippingAsync`, `UpdateFreeShippingAsync`, `GetFreeShippingAsync`, `GetFreeShippingListAsync`, `ActivateFreeShippingAsync`, `DeactivateFreeShippingAsync`, `GetFreeShippingProductsAsync`, `AddFreeShippingSkuAsync`, `RemoveFreeShippingSkuAsync`.
  - **FlexiCombo (8):** `CreateFlexiComboAsync`, `GetFlexiComboDetailsAsync`, `UpdateFlexiComboAsync`, `GetFlexiComboListAsync`, `ActivateFlexiComboAsync`, `DeactivateFlexiComboAsync`, `AddFlexiComboProductsAsync`, `RemoveFlexiComboProductsAsync`.
- Models under `Laz.Sdk.Models.Promotions` namespace for all request/response types.

### Added (Media Center / Video API)

- `client.Media` — new `IMediaService` / `MediaService` group wrapping 7 endpoints:
  - `InitCreateVideoAsync(...)` — `/media/video/block/create` — initialize a video upload session.
  - `UploadVideoBlockAsync(...)` — `/media/video/block/upload` — upload a video block.
  - `CompleteCreateVideoAsync(...)` — `/media/video/block/commit` — complete a video upload session.
  - `GetVideoAsync(...)` — `/media/video/get` — get video details.
  - `RemoveVideoAsync(...)` — `/media/video/remove` — remove a video.
  - `GetVideoQuotaAsync(...)` — `/media/video/quota/get` — get seller's video quota.
  - `UploadImageAsync(...)` — `/image/upload` — upload an image.
- Models under `Laz.Sdk.Models.Media` namespace for all request/response types.

### Added (Product Review API)

- `client.Reviews` — new `IReviewsService` / `ReviewsService` group wrapping 3 endpoints:
  - `GetHistoryReviewIdListAsync(...)` — `/review/seller/history/list` — get paginated review ID list for a product.
  - `GetReviewListByIdListAsync(...)` — `/review/seller/list/v2` — get full review details by review IDs.
  - `SubmitSellerReplyAsync(...)` — `/review/seller/reply/add` — submit a seller reply to a review.
- Models under `Laz.Sdk.Models.Reviews` namespace for all request/response types.

### Added (Finance API)

- `client.Finance` — new `IFinanceService` / `FinanceService` group wrapping 4 endpoints:
  - `GetPayoutStatusAsync(...)` — `/finance/payout/status/get` — payout statements filtered by creation date.
  - `QueryAccountTransactionsAsync(...)` — `/finance/transaction/accountTransactions/query` — paginated account transactions.
  - `QueryLogisticsFeeDetailAsync(...)` — `/lbs/slb/queryLogisticsFeeDetail` — logistics fee details.
  - `QueryTransactionDetailsAsync(...)` — `/finance/seller/transaction/detail` — seller transaction details within a date range.

### Added (Returns &amp; Refund API)

- `client.Returns` — new `IReturnsService` / `ReturnsService` group wrapping 7 endpoints:
  - `GetReverseOrderDetailAsync(...)` — `/order/reverse/return/detail/list` — detailed reverse order info.
  - `GetReverseOrderHistoryListAsync(...)` — `/order/reverse/return/history/list` — paginated history for a reverse order line.
  - `GetReverseOrderReasonListAsync(...)` — `/order/reverse/reason/list` — available reason list.
  - `GetReverseOrdersForSellerAsync(...)` — `/reverse/getreverseordersforseller` — filtered/paged reverse order listing.
  - `ReverseOrderCancelValidateAsync(...)` — `/order/reverse/cancel/validate` — validate cancellation eligibility.
  - `InitReverseOrderCancelAsync(...)` — `/order/reverse/cancel/create` — initiate a cancellation.
  - `ReverseOrderReturnUpdateAsync(...)` — `/order/reverse/return/update` — accept / refund / reject a return.
- Models under `Laz.Sdk.Models.Returns` namespace for all request/response types.

### Added (Product API)

- `client.Products` — new `IProductService` / `ProductService` group wrapping 26 endpoints:
  - `CreateProductAsync(...)` — `/product/create` — create a new product.
  - `UpdateProductAsync(...)` — `/product/update` — update an existing product.
  - `DeactivateProductAsync(...)` — `/product/deactivate` — deactivate a product.
  - `GetProductsAsync(...)` — `/products/get` — list products with filters.
  - `GetProductItemAsync(...)` — `/product/item/get` — get a single product item.
  - `AdjustSellableQuantityAsync(...)` — `/product/stock/sellable/adjust` — adjust sellable stock.
  - `UpdateSellableQuantityAsync(...)` — `/product/stock/sellable/update` — update sellable stock.
  - `UpdatePriceAsync(...)` — `/price/sellable/update` — update sellable price.
  - `SetImagesAsync(...)` — `/images/set` — set product images.
  - `GetCategoryTreeAsync(...)` — `/category/tree/get` — get the category tree.
  - `GetCategoryAttributesAsync(...)` — `/category/attributes/get` — get category attributes.
  - `GetCategorySuggestionAsync(...)` — `/category/suggestion/get` — suggest categories.
  - `GetCategorySuggestionBulkAsync(...)` — `/category/suggestion/bulk/get` — bulk suggest categories.
  - `GetBrandsAsync(...)` — `/brands/get` — get brands for a category.
  - `BatchUpdateSizeChartAsync(...)` — `/size/chart/batch/update` — batch update size charts.
  - `GetQcStatusAsync(...)` — `/product/qc/status/get` — get QC status.
  - `GetQcAlertProductsAsync(...)` — `/product/qc/alert/products/get` — get QC alert products.
  - `GetPreQcRulesAsync(...)` — `/product/pre/qc/rules/get` — get pre-QC rules.
  - `GetProductResponseAsync(...)` — `/product/response/get` — get product response.
  - `GetSellerItemLimitAsync(...)` — `/product/seller/item/limit/get` — get seller item limit.
  - `GetUnfilledAttributeItemAsync(...)` — `/product/unfilled/attribute/item/get` — get unfilled attribute items.
  - `GetProductContentScoreAsync(...)` — `/product/content/score/get` — get product content score.
  - `ProductCheckAsync(...)` — `/product/check` — check a product before creation.
  - `QueryProductExperimentConfigAsync(...)` — `/product/experiment/config/query` — query experiment config.
  - `ExitExperimentAsync(...)` — `/product/experiment/exit` — exit a product experiment.
  - `GetNextCascadePropAsync(...)` — `/product/next/cascade/prop/get` — get next cascade property.
- Models under `Laz.Sdk.Models.Products` namespace for all request/response types.

### Added (Seller API)

- `client.Seller` — new `ISellerService` / `SellerService` group wrapping 13 endpoints:
  - `GetSellerAsync(...)` — `/seller/get` — get seller information.
  - `GetSellerMetricsAsync(...)` — `/seller/metrics/get` — get seller metrics.
  - `GetSellerPerformanceAsync(...)` — `/seller/performance/get` — get seller performance indicators.
  - `BatchQueryFollowStatusAsync(...)` — `/shop/follow/status/batch/query` — batch query follow status.
  - `GetPickUpStoreListAsync(...)` — `/rc/store/list/get` — get pickup store list.
  - `GetWarehouseBySellerIdAsync(...)` — `/warehouse/seller/get` — get warehouse list by seller id.
  - `QueryWarehouseDetailAsync(...)` — `/warehouse/detail/query` — query warehouse detail.
  - `SellerPolicyFetchAsync(...)` — `/seller/policy/fetch` — fetch seller policy.
  - `GetSellerRegisterInfoAsync(...)` — `/seller/register/info/get` — get seller register info.
  - `GetSubAddressAsync(...)` — `/seller/address/sub/get` — get sub-address.
  - `PaymentBindingAsync(...)` — `/seller/payment/binding` — bind payment method.
  - `SellerFieldVerifyAsync(...)` — `/seller/field/verify` — verify a seller field.
  - `GetCountryInfoAsync(...)` — `/seller/country/info/get` — get country info.
- Models under `Laz.Sdk.Models.Seller` namespace for all request/response types.

### Added (Cross Border Product API)

- `client.CrossBorder` — new `ICrossBorderService` / `CrossBorderService` group wrapping 4 endpoints:
  - `CreateGlobalProductAsync(...)` — `/product/global/create` — create a global product (XML payload).
  - `GetGlobalProductExtensionAsync(...)` — `/product/global/extension` — get global product extension info.
  - `UpdateGlobalSkuAsync(...)` — `/product/global/sku/update` — update SKU for a global product.
  - `GetGlobalSellerStatusAsync(...)` — `/product/global/seller/status` — check if seller is cross-border enabled.
- Models under `Laz.Sdk.Models.CrossBorder` namespace for all request/response types.

### Added (Store Decoration API)

- `client.Store` — new `IStoreService` / `StoreService` group wrapping 1 endpoint:
  - `GetStoreCustomPageAsync(...)` — `/store/custom/page/get` — get paginated store custom pages.
- Models under `Laz.Sdk.Models.Store` namespace for all request/response types.

### Added (System API)

- `client.System` — new `ISystemService` / `SystemService` group wrapping 2 endpoints:
  - `CreateAccessTokenWithOpenIdAsync(...)` — `/auth/token/createWithOpenId` — create access token with OpenId.
  - `GetDataMopFormatAsync(...)` — `/data/mop/format/get` — get data format for bulk operations.
- Models under `Laz.Sdk.Models.System` namespace for all request/response types.

### Changed

- `LazClientOptions.AppKey` / `LazClientOptions.AppSecret` are now **optional** (default empty). Multi-tenant apps can register a client with only `ServerUrl` and supply credentials per call via `WithCredentials`. If both DI options and per-call credentials are missing, `ILazClient.ExecuteAsync` (and every typed wrapper) throws `InvalidOperationException` at call time with a clear message instead of failing at startup.
- `client.Auth.CreateAccessTokenAsync(code, ct)` — typed wrapper around `/auth/token/create` on the Lazada auth gateway. Returns `LazAccessToken` with access + refresh tokens, expiry seconds, country, account info, and `country_user_info` list.
- `client.Auth.RefreshAccessTokenAsync(refreshToken, ct)` — typed wrapper around `/auth/token/refresh`.
- `LazAccessToken` + `LazCountryUserInfo` record types under `Laz.Sdk.Models`.
- `LazResponse.ReadAs<T>(JsonSerializerOptions?)` helper for deserializing the raw body of any `ExecuteAsync` call.
- Service-grouped surface: every API domain lives under a property on `ILazClient`. Currently exposes `Auth` (`IAuthService`) and `Orders` (`IOrdersService`).
- `client.Orders.GetOrdersAsync(...)` — `/orders/get` (filtered/sorted/paged listing; full `LazOrder` schema with addresses + recipient info).
- `client.Orders.GetOrderAsync(...)` — `/order/get`.
- `client.Orders.GetOrderItemsAsync(...)` — `/order/items/get` (full ~60-field `LazOrderItem` + nested `LazPickUpStoreInfo`).
- `client.Orders.GetMultipleOrderItemsAsync(...)` — `/orders/items/get` (up to 50 orders per call, grouped response).
- `client.Orders.GetDocumentAsync(request, accessToken, ct)` — `/order/document/get`.
- `client.Orders.PackAsync(...)` — `/order/pack` (mark items packed under a shipping provider).
- `client.Orders.ReadyToShipAsync(...)` — `/order/rts` (assign tracking + mark RTS).
- `client.Orders.CancelAsync(...)` — `/order/cancel` (single-item cancel with reason).
- `Laz.Sdk.Json.StringOrBoolJsonConverter` (internal) — handles Lazada's inconsistent boolean wire forms: `true`/`false` (native), `"true"`/`"false"` (any case), `"1"`/`"0"`, and JSON numbers.
- `LazAddress.AddressDistrict` computed property — gracefully reads either the `addressDsitrict` (typo, used by `/orders/get`) or `addressDistrict` (correct, used by `/order/get`) wire keys. Returns invoice / shipping label / carrier manifest (Base64 file payload + mime type). Wraps `OrderDocumentType` enum to the wire-format string. Helper `OrderDocument.GetFileBytes()` decodes Base64 to bytes.

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
