namespace Laz.Sdk.Models.Orders;

/// <summary>
/// Request for <c>/order/document/get</c>.
/// </summary>
public sealed record GetOrderDocumentRequest
{
    /// <summary>Document type to retrieve.</summary>
    public required OrderDocumentType DocType { get; init; }

    /// <summary>
    /// Order item ids to include in the document. Wired as a JSON array (e.g. <c>[279709, 279710]</c>).
    /// Must contain at least one id.
    /// </summary>
    public required IReadOnlyCollection<long> OrderItemIds { get; init; }
}
