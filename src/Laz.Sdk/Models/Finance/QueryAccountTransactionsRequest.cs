namespace Laz.Sdk.Models.Finance;

/// <summary>
/// Request for <c>/finance/transaction/accountTransactions/query</c>.
/// </summary>
public sealed record QueryAccountTransactionsRequest
{
    /// <summary>Transaction type filter (optional).</summary>
    public string? TransactionType { get; init; }

    /// <summary>Sub-transaction type filter (optional).</summary>
    public string? SubTransactionType { get; init; }

    /// <summary>Transaction number filter (optional).</summary>
    public string? TransactionNumber { get; init; }

    /// <summary>Required. Page size.</summary>
    public int PageSize { get; init; }

    /// <summary>Required. Start time in yyyyMMdd format.</summary>
    public string StartTime { get; init; } = string.Empty;

    /// <summary>Required. End time in yyyyMMdd format.</summary>
    public string EndTime { get; init; } = string.Empty;

    /// <summary>Required. Page number (1-based).</summary>
    public int PageNum { get; init; }
}
