namespace Laz.Sdk.Models.Finance;

/// <summary>
/// Request for <c>/finance/seller/transaction/detail</c>.
/// </summary>
public sealed record QueryTransactionDetailsRequest
{
    /// <summary>Required. Start date (e.g. "2018-01-01").</summary>
    public string StartDate { get; init; } = string.Empty;

    /// <summary>Required. End date (e.g. "2018-01-31").</summary>
    public string EndDate { get; init; } = string.Empty;

    /// <summary>Optional page number.</summary>
    public int? PageNum { get; init; }

    /// <summary>Optional page size.</summary>
    public int? PageSize { get; init; }
}
