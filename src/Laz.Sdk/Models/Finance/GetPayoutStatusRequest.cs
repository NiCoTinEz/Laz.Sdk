namespace Laz.Sdk.Models.Finance;

/// <summary>
/// Request for <c>/finance/payout/status/get</c>.
/// </summary>
public sealed record GetPayoutStatusRequest
{
    /// <summary>Required. Filter payouts created on or after this date (e.g. "2018-01-01").</summary>
    public string CreatedAfter { get; init; } = string.Empty;
}
