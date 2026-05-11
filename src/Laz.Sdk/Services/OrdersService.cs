namespace Laz.Sdk.Services;

internal sealed class OrdersService(ILazClient client) : IOrdersService
{
    // Reserved for future use — implementations call `client.ExecuteAsync(...)`
    // and `LazResponseExtensions.DeserializeOrThrow<T>()`.
    private readonly ILazClient _client = client;
}
