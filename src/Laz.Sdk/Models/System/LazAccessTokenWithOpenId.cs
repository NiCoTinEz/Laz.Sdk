using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.System;

/// <summary>Response from <c>/auth/token/createWithOpenId</c>.</summary>
public sealed record LazAccessTokenWithOpenId
{
    [JsonPropertyName("access_token")]       public string? AccessToken { get; init; }
    [JsonPropertyName("refresh_token")]      public string? RefreshToken { get; init; }

    [JsonPropertyName("expires_in")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long ExpiresIn { get; init; }

    [JsonPropertyName("refresh_expires_in")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long RefreshExpiresIn { get; init; }

    [JsonPropertyName("country")]            public string? Country { get; init; }
    [JsonPropertyName("account_platform")]   public string? AccountPlatform { get; init; }
    [JsonPropertyName("account")]            public string? Account { get; init; }
    [JsonPropertyName("account_id")]         public string? AccountId { get; init; }
    [JsonPropertyName("open_id")]            public string? OpenId { get; init; }

    [JsonPropertyName("country_user_info")]
    public IReadOnlyList<LazCountryUserInfo>? CountryUserInfo { get; init; }

    [JsonPropertyName("code")]               public string? Code { get; init; }
    [JsonPropertyName("type")]               public string? Type { get; init; }
    [JsonPropertyName("message")]            public string? Message { get; init; }
    [JsonPropertyName("request_id")]         public string? RequestId { get; init; }
}

/// <summary>Response for <c>/data/mop/format/get</c>.</summary>
public sealed record GetDataMopFormatResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public DataMopFormatData? Data { get; init; }
}

/// <summary>Inner <c>data</c> object for <see cref="GetDataMopFormatResponse"/>.</summary>
public sealed record DataMopFormatData
{
    [JsonPropertyName("format")] public string? Format { get; init; }
}
