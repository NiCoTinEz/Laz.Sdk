namespace Laz.Sdk.Models.Orders;

/// <summary>
/// Supported document types for <c>/order/document/get</c>.
/// </summary>
public enum OrderDocumentType
{
    /// <summary>Tax invoice. Wire value: <c>"invoice"</c>.</summary>
    Invoice,

    /// <summary>Shipping label (AWB). Wire value: <c>"shippingLabel"</c>.</summary>
    ShippingLabel,

    /// <summary>Carrier manifest. Wire value: <c>"carrierManifest"</c>.</summary>
    CarrierManifest,
}
