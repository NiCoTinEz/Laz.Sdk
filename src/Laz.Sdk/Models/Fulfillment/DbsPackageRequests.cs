namespace Laz.Sdk.Models.Fulfillment;

/// <summary>Request for <c>/order/package/sof/delivered</c> (ConfirmDeliveryForDBS).</summary>
public sealed record ConfirmDbsDeliveryRequest
{
    public required IReadOnlyList<AwbPackage> Packages { get; init; }
}

/// <summary>Request for <c>/order/package/sof/failed_delivery</c> (FailedDeliveryForDBS).</summary>
public sealed record FailedDbsDeliveryRequest
{
    public required IReadOnlyList<AwbPackage> Packages { get; init; }
}
