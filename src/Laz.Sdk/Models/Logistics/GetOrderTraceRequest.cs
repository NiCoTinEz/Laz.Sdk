namespace Laz.Sdk.Models.Logistics;

/// <summary>Request for <c>/logistic/order/trace</c>.</summary>
public sealed record GetOrderTraceRequest
{
    /// <summary>Seller order id.</summary>
    public required string OrderId { get; init; }

    /// <summary>Optional locale (e.g. <c>"en"</c>, <c>"th"</c>) for human-readable status text.</summary>
    public string? Locale { get; init; }

    /// <summary>Optional OFC package id list. Wired as <c>[id1,id2]</c>.</summary>
    public IReadOnlyCollection<string>? OfcPackageIdList { get; init; }
}
