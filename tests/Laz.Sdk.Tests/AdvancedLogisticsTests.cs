using Laz.Sdk.Models.Logistics;
using Laz.Sdk.Util;
using Xunit;

namespace Laz.Sdk.Tests;

public class AdvancedLogisticsTests
{
    private static LazClient NewClient(TestHandler handler)
        => new(new HttpClient(handler), new LazClientOptions { AppKey = "ak", AppSecret = "as" });

    private const string LogisticsOpBody = """
    {
      "retryable":"false",
      "code":"0",
      "success":"true",
      "errorMessage":"traceId=x",
      "errorCode":"BAD_REQUEST",
      "request_id":"r",
      "errors":[{"field":"$.name","errorMessage":"$.name is missing","errorCode":"INVALID_PARAMETER"}]
    }
    """;

    [Fact]
    public async Task AddOrUpdatePickupStopAsync_PostsExpectedBody()
    {
        var handler = new TestHandler(LogisticsOpBody);
        var client = NewClient(handler);

        var response = await client.Logistics.AddOrUpdatePickupStopAsync(
            new AddOrUpdatePickupStopRequest
            {
                StopId           = "Stop001",
                SellerId         = "200165961111",
                WarehouseCode    = "dropshipping",
                DopStationId     = "SSG",
                DopStationName   = "Sai Gon",
                PickupType       = "Pickup",
                Status           = "planned",
                StatusUpdateTime = 1659439136265L,
                DriverName       = "John Wick",
                Eta              = 1659439136265L,
                SuccessVolume    = "10",
                FailedVolume     = "1",
                FailedVolumeList = new[]
                {
                    new PickupFailedVolume { Type = "Failed", Volume = "1", Reason = "Seller closed" },
                },
            },
            "tok");

        Assert.True(response.Success);
        Assert.Single(response.Errors!);
        var body = handler.LastRequestBody!;
        Assert.Contains("stopId=Stop001",                    body, StringComparison.Ordinal);
        Assert.Contains("status=planned",                    body, StringComparison.Ordinal);
        Assert.Contains("statusUpdateTime=1659439136265",    body, StringComparison.Ordinal);
        Assert.Contains("failedVolumeList=",                 body, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Create3PLStationAsync_PostsAllFields()
    {
        var handler = new TestHandler(LogisticsOpBody);
        var client = NewClient(handler);

        await client.Logistics.Create3PLStationAsync(
            new Create3PLStationRequest
            {
                ExternalCode      = "NJV_001",
                Name              = "Station 001",
                FunctionCodes     = new[] { "CP" },
                SubTypes          = new[] { "COLLECTION_ON_POINT" },
                CodSupport        = true,
                Age               = 10,
                FirstMileTplSlugs = new[] { "ninjavan-id" },
                LastMileTplSlugs  = new[] { "ninjavan-id" },
                Contact           = new StationContact { Name = "Zohan", Phone = "+84000000000", Email = "z@x.com" },
                Address           = new StationAddress { Details = "addr", Latitude = "10", Longitude = "113" },
                TimeZone          = "+08:00",
            },
            "tok");

        var body = handler.LastRequestBody!;
        Assert.Contains("externalCode=NJV_001",     body, StringComparison.Ordinal);
        Assert.Contains("codSupport=true",          body, StringComparison.Ordinal);
        Assert.Contains("age=10",                   body, StringComparison.Ordinal);
        Assert.Contains("functionCodes=",           body, StringComparison.Ordinal);
        Assert.Contains("contact=",                 body, StringComparison.Ordinal);
        Assert.Contains("address=",                 body, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Update3PLStationAsync_PostsEnableFlag()
    {
        var handler = new TestHandler(LogisticsOpBody);
        var client = NewClient(handler);

        await client.Logistics.Update3PLStationAsync(
            new Update3PLStationRequest
            {
                ExternalCode      = "NJV_001",
                Enable            = true,
                FunctionCodes     = new[] { "CP" },
                SubTypes          = new[] { "COLLECTION_ON_POINT" },
                CodSupport        = true,
                FirstMileTplSlugs = new[] { "ninjavan-id" },
                LastMileTplSlugs  = new[] { "ninjavan-id" },
                Contact           = new StationContact { Name = "Z", Phone = "+1" },
                Address           = new StationAddress { Details = "addr" },
            },
            "tok");

        Assert.Contains("enable=true", handler.LastRequestBody!, StringComparison.Ordinal);
    }

    [Fact]
    public async Task UpdatePickupTimeSlotAsync_PostsTimeslots()
    {
        var handler = new TestHandler(LogisticsOpBody);
        var client = NewClient(handler);

        await client.Logistics.UpdatePickupTimeSlotAsync(
            new UpdatePickupTimeSlotRequest
            {
                SellerId        = "200165961111",
                WarehouseCode   = "dropshipping",
                PickupTimeslots = new[] { "08:00-12:00", "13:00-15:00" },
            },
            "tok");

        var body = handler.LastRequestBody!;
        Assert.Contains("sellerId=200165961111",  body, StringComparison.Ordinal);
        Assert.Contains("pickupTimeslots=",       body, StringComparison.Ordinal);
    }

    [Fact]
    public async Task ScanParcelAsync_RoundTripsTrackingNumber()
    {
        const string Body = """{"code":"0","trackingNumber":"MYMPA092974023","request_id":"r"}""";
        var handler = new TestHandler(Body);
        var client = NewClient(handler);

        var response = await client.Logistics.ScanParcelAsync(
            new ScanParcelRequest { CageNumber = "case1", TrackingNumber = "MYMPA092974023" },
            "tok");

        Assert.Equal("MYMPA092974023", response.TrackingNumber);
        var body = handler.LastRequestBody!;
        Assert.Contains("cageNumber=case1",             body, StringComparison.Ordinal);
        Assert.Contains("trackingNumber=MYMPA092974023", body, StringComparison.Ordinal);
    }

    [Fact]
    public async Task StationDopScanAsync_ParsesNestedData()
    {
        const string Body = """
        {
          "code":"0",
          "data":{"trackingNumber":"TRACKING1"},
          "success":"true",
          "error":{"errorCode":"ERROR"},
          "request_id":"r"
        }
        """;
        var client = NewClient(new TestHandler(Body));

        var response = await client.Logistics.StationDopScanAsync(
            new StationDopScanRequest { CageNumber = "CASE1", TrackingNumber = "TRACKING1" },
            "tok");

        Assert.True(response.Success);
        Assert.Equal("TRACKING1", response.Data!.TrackingNumber);
        Assert.Equal("ERROR",     response.Error!.ErrorCode);
    }

    [Fact]
    public async Task CreateConsolidationServiceAsync_PostsUnitCodes_AndProperties()
    {
        const string Body = """{"code":"0","data":null,"success":"false","errorCode":"X","errorMsg":"y","request_id":"r"}""";
        var handler = new TestHandler(Body);
        var client = NewClient(handler);

        var response = await client.Logistics.CreateConsolidationServiceAsync(
            new CreateConsolidationServiceRequest
            {
                UnitCodes  = new[] { "FU23900000000000007164132600", "FU2386993" },
                Properties = new Dictionary<string, string> { ["sellerGroupName"] = "CN-Others" },
            },
            "tok");

        Assert.False(response.Success);
        var body = handler.LastRequestBody!;
        Assert.Contains("unitCodes=",   body, StringComparison.Ordinal);
        Assert.Contains("properties=",  body, StringComparison.Ordinal);
    }

    [Fact]
    public async Task UpdateLastMileAsync_PostsThreeFields()
    {
        const string Body = """{"code":"0","data":"*****","success":"false","errorCode":"X","errorMsg":"y","request_id":"r"}""";
        var handler = new TestHandler(Body);
        var client = NewClient(handler);

        var response = await client.Logistics.UpdateLastMileAsync(
            new UpdateLastMileRequest
            {
                UnitCode             = "FU20202020001",
                ShippingProviderCode = "057_***_****",
                TrackingNumber       = "TN_0001",
            },
            "tok");

        Assert.Equal("*****", response.Data);
        var body = handler.LastRequestBody!;
        Assert.Contains("unitCode=FU20202020001",   body, StringComparison.Ordinal);
        Assert.Contains("trackingNumber=TN_0001",   body, StringComparison.Ordinal);
    }
}
