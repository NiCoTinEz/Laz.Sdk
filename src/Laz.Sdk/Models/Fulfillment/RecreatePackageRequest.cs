namespace Laz.Sdk.Models.Fulfillment;

/// <summary>Request for <c>/order/package/repack</c>.</summary>
public sealed record RecreatePackageRequest
{
    /// <summary>Packages to repack.</summary>
    public required IReadOnlyList<AwbPackage> Packages { get; init; }
}
