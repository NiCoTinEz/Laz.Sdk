namespace Laz.Sdk.Models.Fulfillment;

/// <summary>Request for <c>/order/package/document/get</c> (PrintAWB).</summary>
public sealed record PrintAwbRequest
{
    /// <summary>Document format. Default <c>"PDF"</c>.</summary>
    public string DocType { get; init; } = "PDF";

    /// <summary>Whether to print the item list. Wire form is the string <c>"true"</c> / <c>"false"</c>.</summary>
    public bool PrintItemList { get; init; }

    /// <summary>Packages to print. Must contain at least one entry.</summary>
    public required IReadOnlyList<AwbPackage> Packages { get; init; }
}

/// <summary>Single package in a <see cref="PrintAwbRequest"/>.</summary>
public sealed record AwbPackage
{
    public required string PackageId { get; init; }
}
