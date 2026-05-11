using Laz.Sdk.Models.Logistics;

namespace Laz.Sdk.Services;

/// <summary>
/// Wraps Lazada's <c>Logistics API</c> family — order tracking, 3PL station / runsheet
/// management, parcel scanning, and LDP cross-border last-mile updates. Access via
/// <see cref="ILazClient.Logistics"/>.
/// </summary>
public interface ILogisticsService
{
    /// <summary>Get parcel tracking events for an order. Calls <c>/logistic/order/trace</c>.</summary>
    Task<GetOrderTraceResponse> GetOrderTraceAsync(
        GetOrderTraceRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>3PL pushes pickup-stop status to TPS. Calls <c>/logistics/tps/runsheets/stops</c>.</summary>
    Task<LogisticsOperationResponse> AddOrUpdatePickupStopAsync(
        AddOrUpdatePickupStopRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>External partner creates a 3PL station in TPS. Calls <c>/logistics/tps/stations/create</c>.</summary>
    Task<LogisticsOperationResponse> Create3PLStationAsync(
        Create3PLStationRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Update an existing 3PL station. Calls <c>/logistics/tps/stations/update</c>.</summary>
    Task<LogisticsOperationResponse> Update3PLStationAsync(
        Update3PLStationRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Update seller pickup time slots. Calls <c>/logistics/tps/sellers/pickup_timeslot</c>.</summary>
    Task<LogisticsOperationResponse> UpdatePickupTimeSlotAsync(
        UpdatePickupTimeSlotRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Scan a parcel into a cage. Calls <c>/dop/scan</c>.</summary>
    Task<ScanParcelResponse> ScanParcelAsync(
        ScanParcelRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Station DOP scan variant. Calls <c>/stations/dop/scan</c>.</summary>
    Task<StationDopScanResponse> StationDopScanAsync(
        StationDopScanRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Create LDP consolidation service. Calls <c>/logistics/ldp/createConsolidationService</c>.</summary>
    Task<LdpOperationResponse> CreateConsolidationServiceAsync(
        CreateConsolidationServiceRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Update LDP last-mile tracking. Calls <c>/logistics/ldp/updateLastmile</c>.</summary>
    Task<LdpOperationResponse> UpdateLastMileAsync(
        UpdateLastMileRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);
}
