namespace Laz.Sdk.Models.Logistics;

/// <summary>
/// Request for <c>/logistics/tps/stations/update</c>. Same shape as
/// <see cref="Create3PLStationRequest"/> plus a required <see cref="Enable"/> flag
/// and an optional <see cref="Name"/>.
/// </summary>
public sealed record Update3PLStationRequest
{
    public required string ExternalCode { get; init; }
    public string? Modifier { get; init; }
    public required bool Enable { get; init; }
    public string? Name { get; init; }
    public required IReadOnlyList<string> FunctionCodes { get; init; }
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
