using System.Globalization;
using System.Text.Json;
using Laz.Sdk.Models.Logistics;
using Laz.Sdk.Util;

namespace Laz.Sdk.Services;

internal sealed class LogisticsService(LazClient client) : ILogisticsService
{
    private readonly LazClient _client = client;

    public async Task<GetOrderTraceResponse> GetOrderTraceAsync(
        GetOrderTraceRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);
        ArgumentException.ThrowIfNullOrEmpty(request.OrderId);

        var lazRequest = new LazRequest("/logistic/order/trace") { HttpMethod = Constants.METHOD_GET };
        lazRequest.AddApiParameter("order_id", request.OrderId);
        if (!string.IsNullOrEmpty(request.Locale))
        {
            lazRequest.AddApiParameter("locale", request.Locale);
        }
        lazRequest.AddApiParameter(
            "ofcPackageIdList",
            request.OfcPackageIdList is { Count: > 0 } ids ? "[" + string.Join(',', ids) + "]" : "[]");

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetOrderTraceResponse>();
    }

    public Task<LogisticsOperationResponse> AddOrUpdatePickupStopAsync(
        AddOrUpdatePickupStopRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/logistics/tps/runsheets/stops");
        lazRequest.AddApiParameter("stopId",            request.StopId);
        lazRequest.AddApiParameter("sellerId",          request.SellerId);
        lazRequest.AddApiParameter("warehouseCode",     request.WarehouseCode);
        lazRequest.AddApiParameter("dopStationId",      request.DopStationId);
        lazRequest.AddApiParameter("dopStationName",    request.DopStationName);
        lazRequest.AddApiParameter("pickupType",        request.PickupType);
        lazRequest.AddApiParameter("status",            request.Status);
        lazRequest.AddApiParameter("statusUpdateTime",  request.StatusUpdateTime.ToString(CultureInfo.InvariantCulture));
        lazRequest.AddApiParameter("dispatcherName",    request.DispatcherName);
        lazRequest.AddApiParameter("dispatcherContact", request.DispatcherContact);
        lazRequest.AddApiParameter("driverId",          request.DriverId);
        lazRequest.AddApiParameter("driverName",        request.DriverName);
        lazRequest.AddApiParameter("driverContact",     request.DriverContact);
        if (request.Eta is { } eta)
        {
            lazRequest.AddApiParameter("eta", eta.ToString(CultureInfo.InvariantCulture));
        }
        lazRequest.AddApiParameter("successVolume",     request.SuccessVolume);
        lazRequest.AddApiParameter("failedVolume",      request.FailedVolume);
        if (request.FailedVolumeList is { Count: > 0 } fvl)
        {
            lazRequest.AddApiParameter("failedVolumeList", JsonSerializer.Serialize(fvl));
        }

        return ExecuteAndDeserialize<LogisticsOperationResponse>(lazRequest, accessToken, credentials, cancellationToken);
    }

    public Task<LogisticsOperationResponse> Create3PLStationAsync(
        Create3PLStationRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/logistics/tps/stations/create");
        AddStationCommonParams(
            lazRequest,
            externalCode: request.ExternalCode,
            modifier: request.Modifier,
            name: request.Name,
            enable: null,
            functionCodes: request.FunctionCodes,
            subTypes: request.SubTypes,
            codSupport: request.CodSupport,
            age: request.Age,
            firstMileTplSlugs: request.FirstMileTplSlugs,
            lastMileTplSlugs: request.LastMileTplSlugs,
            contact: request.Contact,
            address: request.Address,
            timeZone: request.TimeZone,
            schedules: request.Schedules,
            constraints: request.Constraints);

        return ExecuteAndDeserialize<LogisticsOperationResponse>(lazRequest, accessToken, credentials, cancellationToken);
    }

    public Task<LogisticsOperationResponse> Update3PLStationAsync(
        Update3PLStationRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/logistics/tps/stations/update");
        AddStationCommonParams(
            lazRequest,
            externalCode: request.ExternalCode,
            modifier: request.Modifier,
            name: request.Name,
            enable: request.Enable,
            functionCodes: request.FunctionCodes,
            subTypes: request.SubTypes,
            codSupport: request.CodSupport,
            age: request.Age,
            firstMileTplSlugs: request.FirstMileTplSlugs,
            lastMileTplSlugs: request.LastMileTplSlugs,
            contact: request.Contact,
            address: request.Address,
            timeZone: request.TimeZone,
            schedules: request.Schedules,
            constraints: request.Constraints);

        return ExecuteAndDeserialize<LogisticsOperationResponse>(lazRequest, accessToken, credentials, cancellationToken);
    }

    public Task<LogisticsOperationResponse> UpdatePickupTimeSlotAsync(
        UpdatePickupTimeSlotRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);
        if (request.PickupTimeslots is null || request.PickupTimeslots.Count == 0)
        {
            throw new ArgumentException("PickupTimeslots must contain at least one entry.", nameof(request));
        }

        var lazRequest = new LazRequest("/logistics/tps/sellers/pickup_timeslot");
        lazRequest.AddApiParameter("sellerId",        request.SellerId);
        lazRequest.AddApiParameter("warehouseCode",   request.WarehouseCode);
        lazRequest.AddApiParameter("pickupTimeslots", JsonSerializer.Serialize(request.PickupTimeslots));

        return ExecuteAndDeserialize<LogisticsOperationResponse>(lazRequest, accessToken, credentials, cancellationToken);
    }

    public Task<ScanParcelResponse> ScanParcelAsync(
        ScanParcelRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);
        ArgumentException.ThrowIfNullOrEmpty(request.CageNumber);
        ArgumentException.ThrowIfNullOrEmpty(request.TrackingNumber);

        var lazRequest = new LazRequest("/dop/scan");
        lazRequest.AddApiParameter("cageNumber",     request.CageNumber);
        lazRequest.AddApiParameter("trackingNumber", request.TrackingNumber);

        return ExecuteAndDeserialize<ScanParcelResponse>(lazRequest, accessToken, credentials, cancellationToken);
    }

    public Task<StationDopScanResponse> StationDopScanAsync(
        StationDopScanRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);
        ArgumentException.ThrowIfNullOrEmpty(request.CageNumber);
        ArgumentException.ThrowIfNullOrEmpty(request.TrackingNumber);

        var lazRequest = new LazRequest("/stations/dop/scan");
        lazRequest.AddApiParameter("cageNumber",     request.CageNumber);
        lazRequest.AddApiParameter("trackingNumber", request.TrackingNumber);

        return ExecuteAndDeserialize<StationDopScanResponse>(lazRequest, accessToken, credentials, cancellationToken);
    }

    public Task<LdpOperationResponse> CreateConsolidationServiceAsync(
        CreateConsolidationServiceRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);
        if (request.UnitCodes is null || request.UnitCodes.Count == 0)
        {
            throw new ArgumentException("UnitCodes must contain at least one entry.", nameof(request));
        }
        ArgumentNullException.ThrowIfNull(request.Properties);

        var lazRequest = new LazRequest("/logistics/ldp/createConsolidationService");
        lazRequest.AddApiParameter("unitCodes",  JsonSerializer.Serialize(request.UnitCodes));
        lazRequest.AddApiParameter("properties", JsonSerializer.Serialize(request.Properties));

        return ExecuteAndDeserialize<LdpOperationResponse>(lazRequest, accessToken, credentials, cancellationToken);
    }

    public Task<LdpOperationResponse> UpdateLastMileAsync(
        UpdateLastMileRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);
        ArgumentException.ThrowIfNullOrEmpty(request.UnitCode);
        ArgumentException.ThrowIfNullOrEmpty(request.ShippingProviderCode);
        ArgumentException.ThrowIfNullOrEmpty(request.TrackingNumber);

        var lazRequest = new LazRequest("/logistics/ldp/updateLastmile");
        lazRequest.AddApiParameter("unitCode",             request.UnitCode);
        lazRequest.AddApiParameter("shippingProviderCode", request.ShippingProviderCode);
        lazRequest.AddApiParameter("trackingNumber",       request.TrackingNumber);

        return ExecuteAndDeserialize<LdpOperationResponse>(lazRequest, accessToken, credentials, cancellationToken);
    }

    private async Task<TResponse> ExecuteAndDeserialize<TResponse>(
        LazRequest lazRequest,
        string accessToken,
        LazCredentials? credentials,
        CancellationToken cancellationToken)
        where TResponse : class
    {
        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<TResponse>();
    }

    private static void AddStationCommonParams(
        LazRequest lazRequest,
        string externalCode,
        string? modifier,
        string? name,
        bool? enable,
        IReadOnlyList<string> functionCodes,
        IReadOnlyList<string> subTypes,
        bool codSupport,
        int? age,
        IReadOnlyList<string> firstMileTplSlugs,
        IReadOnlyList<string> lastMileTplSlugs,
        StationContact contact,
        StationAddress address,
        string? timeZone,
        IReadOnlyList<StationSchedule>? schedules,
        IReadOnlyList<StationConstraint>? constraints)
    {
        lazRequest.AddApiParameter("externalCode",      externalCode);
        lazRequest.AddApiParameter("modifier",          modifier);
        if (enable is { } e)
        {
            lazRequest.AddApiParameter("enable", e ? "true" : "false");
        }
        lazRequest.AddApiParameter("name",              name);
        lazRequest.AddApiParameter("functionCodes",     JsonSerializer.Serialize(functionCodes));
        lazRequest.AddApiParameter("subTypes",          JsonSerializer.Serialize(subTypes));
        lazRequest.AddApiParameter("codSupport",        codSupport ? "true" : "false");
        if (age is { } a)
        {
            lazRequest.AddApiParameter("age", a.ToString(CultureInfo.InvariantCulture));
        }
        lazRequest.AddApiParameter("firstMileTplSlugs", JsonSerializer.Serialize(firstMileTplSlugs));
        lazRequest.AddApiParameter("lastMileTplSlugs",  JsonSerializer.Serialize(lastMileTplSlugs));
        lazRequest.AddApiParameter("contact",           JsonSerializer.Serialize(contact));
        lazRequest.AddApiParameter("address",           JsonSerializer.Serialize(address));
        lazRequest.AddApiParameter("timeZone",          timeZone);
        if (schedules is { Count: > 0 })
        {
            lazRequest.AddApiParameter("schedules", JsonSerializer.Serialize(schedules));
        }
        if (constraints is { Count: > 0 })
        {
            lazRequest.AddApiParameter("constraints", JsonSerializer.Serialize(constraints));
        }
    }
}
