using System.Text.Json.Serialization;
using Laz.Sdk.Json;

namespace Laz.Sdk.Models.Finance;

/// <summary>Response envelope for <c>/finance/payout/status/get</c>.</summary>
public sealed record GetPayoutStatusResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public IReadOnlyList<PayoutStatement>? Data { get; init; }
}

/// <summary>A single payout statement as returned by <c>/finance/payout/status/get</c>.</summary>
public sealed record PayoutStatement
{
    [JsonPropertyName("closing_balance")]
    public string? ClosingBalance { get; init; }

    [JsonPropertyName("payout")]
    public string? Payout { get; init; }

    [JsonPropertyName("paid")]
    public string? Paid { get; init; }

    [JsonPropertyName("statement_number")]
    public string? StatementNumber { get; init; }

    [JsonPropertyName("created_at")]
    public string? CreatedAt { get; init; }

    [JsonPropertyName("updated_at")]
    public string? UpdatedAt { get; init; }

    [JsonPropertyName("opening_balance")]
    public string? OpeningBalance { get; init; }

    [JsonPropertyName("item_revenue")]
    public string? ItemRevenue { get; init; }

    [JsonPropertyName("shipment_fee")]
    public string? ShipmentFee { get; init; }

    [JsonPropertyName("refunds")]
    public string? Refunds { get; init; }

    [JsonPropertyName("commission_fee")]
    public string? CommissionFee { get; init; }

    [JsonPropertyName("transaction_fee")]
    public string? TransactionFee { get; init; }

    [JsonPropertyName("other_fee")]
    public string? OtherFee { get; init; }

    [JsonPropertyName("adjustment")]
    public string? Adjustment { get; init; }

    [JsonPropertyName("payment_voucher_number")]
    public string? PaymentVoucherNumber { get; init; }

    [JsonPropertyName("payment_voucher_date")]
    public string? PaymentVoucherDate { get; init; }

    [JsonPropertyName("payment_received")]
    public string? PaymentReceived { get; init; }

    [JsonPropertyName("paid_status")]
    public string? PaidStatus { get; init; }

    [JsonPropertyName("currency")]
    public string? Currency { get; init; }
}
