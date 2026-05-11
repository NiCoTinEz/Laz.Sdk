using System.Text.Json.Serialization;

namespace Laz.Sdk.Models;

/// <summary>
/// Response from <c>/auth/token/create</c> and <c>/auth/token/refresh</c>.
/// On success, <see cref="AccessToken"/> and <see cref="RefreshToken"/> are populated.
/// On failure, <see cref="Code"/> and <see cref="Message"/> describe the error
/// (the client surface throws <see cref="LazException"/> in that case).
/// </summary>
public sealed record LazAccessToken
{
    [JsonPropertyName("access_token")]       public string? AccessToken { get; init; }
    [JsonPropertyName("refresh_token")]      public string? RefreshToken { get; init; }

    /// <summary>Access token lifetime in seconds.</summary>
    [JsonPropertyName("expires_in")]         public long ExpiresIn { get; init; }

    /// <summary>Refresh token lifetime in seconds.</summary>
    [JsonPropertyName("refresh_expires_in")] public long RefreshExpiresIn { get; init; }

    [JsonPropertyName("country")]            public string? Country { get; init; }
    [JsonPropertyName("account_platform")]   public string? AccountPlatform { get; init; }
    [JsonPropertyName("account")]            public string? Account { get; init; }
    [JsonPropertyName("account_id")]         public string? AccountId { get; init; }

    /// <summary>Per-country seller details for multi-country sellers.</summary>
    [JsonPropertyName("country_user_info")]  public IReadOnlyList<LazCountryUserInfo>? CountryUserInfo { get; init; }

    [JsonPropertyName("code")]               public string? Code { get; init; }
    [JsonPropertyName("type")]               public string? Type { get; init; }
    [JsonPropertyName("message")]            public string? Message { get; init; }
    [JsonPropertyName("request_id")]         public string? RequestId { get; init; }
}
