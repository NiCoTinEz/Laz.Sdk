using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Logistics;

/// <summary>Request for <c>/logistics/tps/stations/create</c>.</summary>
public sealed record Create3PLStationRequest
{
    public required string ExternalCode { get; init; }
    public required string Name { get; init; }
    public string? Modifier { get; init; }

    /// <summary>Station functions (e.g. <c>["CP"]</c>, <c>["DOP"]</c>, <c>["Return"]</c>).</summary>
    public required IReadOnlyList<string> FunctionCodes { get; init; }

    /// <summary>Station subtypes (depend on function).</summary>
    public required IReadOnlyList<string> SubTypes { get; init; }

    public required bool CodSupport { get; init; }
    public int? Age { get; init; }
    public required IReadOnlyList<string> FirstMileTplSlugs { get; init; }
    public required IReadOnlyList<string> LastMileTplSlugs { get; init; }
    public required StationContact Contact { get; init; }
    public required StationAddress Address { get; init; }
    public string? TimeZone { get; init; }
    public IReadOnlyList<StationSchedule>? Schedules { get; init; }
    public IReadOnlyList<StationConstraint>? Constraints { get; init; }
}

public sealed record StationContact
{
    [JsonPropertyName("name")]  public required string Name { get; init; }
    [JsonPropertyName("phone")] public required string Phone { get; init; }
    [JsonPropertyName("email")] public string? Email { get; init; }
}

public sealed record StationAddress
{
    [JsonPropertyName("id")]        public string? Id { get; init; }
    [JsonPropertyName("details")]   public required string Details { get; init; }
    [JsonPropertyName("latitude")]  public string? Latitude { get; init; }
    [JsonPropertyName("longitude")] public string? Longitude { get; init; }
}

public sealed record StationSchedule
{
    [JsonPropertyName("workDays")]    public required IReadOnlyList<string> WorkDays { get; init; }
    [JsonPropertyName("startTime")]   public required string StartTime { get; init; }
    [JsonPropertyName("endTime")]     public required string EndTime { get; init; }
    [JsonPropertyName("cutOffTime")]  public string? CutOffTime { get; init; }
}

public sealed record StationConstraint
{
    [JsonPropertyName("functionCode")] public required string FunctionCode { get; init; }
    [JsonPropertyName("maxCapacity")]  public string? MaxCapacity { get; init; }
    [JsonPropertyName("maxCbm")]       public string? MaxCbm { get; init; }
    [JsonPropertyName("maxWeight")]    public string? MaxWeight { get; init; }
    [JsonPropertyName("maxHeight")]    public string? MaxHeight { get; init; }
    [JsonPropertyName("maxLength")]    public string? MaxLength { get; init; }
    [JsonPropertyName("maxWidth")]     public string? MaxWidth { get; init; }
}
