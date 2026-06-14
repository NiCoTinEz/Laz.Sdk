using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Seller;

/// <summary>Request for <c>/seller/payment/binding</c>.</summary>
public sealed record PaymentBindingRequest
{
    /// <summary>Payment method (e.g., "bank_transfer").</summary>
    [JsonPropertyName("payment_method")]
    public string? PaymentMethod { get; init; }

    /// <summary>Bank account number.</summary>
    [JsonPropertyName("account_no")]
    public string? AccountNo { get; init; }

    /// <summary>Bank account holder name.</summary>
    [JsonPropertyName("account_name")]
    public string? AccountName { get; init; }

    /// <summary>Bank code.</summary>
    [JsonPropertyName("bank_code")]
    public string? BankCode { get; init; }

    /// <summary>Bank branch code (optional).</summary>
    [JsonPropertyName("branch_code")]
    public string? BranchCode { get; init; }

    /// <summary>Additional payment details as JSON string.</summary>
    [JsonPropertyName("payment_details")]
    public string? PaymentDetails { get; init; }
}
