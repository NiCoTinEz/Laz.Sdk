using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Finance;

/// <summary>Response envelope for <c>/finance/seller/transaction/detail</c>.</summary>
public sealed record QueryTransactionDetailsResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public TransactionDetailPage? Data { get; init; }
}

/// <summary>Paginated transaction details.</summary>
public sealed record TransactionDetailPage
{
    [JsonPropertyName("total")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int Total { get; init; }

    [JsonPropertyName("currentPage")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int CurrentPage { get; init; }

    [JsonPropertyName("pageSize")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int PageSize { get; init; }

    [JsonPropertyName("list")]
    public IReadOnlyList<TransactionDetail>? List { get; init; }
}

/// <summary>A single transaction detail record.</summary>
public sealed record TransactionDetail
{
    [JsonPropertyName("transaction_number")]
    public string? TransactionNumber { get; init; }

    [JsonPropertyName("transaction_type")]
    public string? TransactionType { get; init; }

    [JsonPropertyName("transaction_time")]
    public string? TransactionTime { get; init; }

    [JsonPropertyName("order_number")]
    public string? OrderNumber { get; init; }

    [JsonPropertyName("debit_amount")]
    public string? DebitAmount { get; init; }

    [JsonPropertyName("credit_amount")]
    public string? CreditAmount { get; init; }

    [JsonPropertyName("balance")]
    public string? Balance { get; init; }

    [JsonPropertyName("remark")]
    public string? Remark { get; init; }

    [JsonPropertyName("currency")]
    public string? Currency { get; init; }
}
