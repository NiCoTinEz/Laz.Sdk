namespace Laz.Sdk;

/// <summary>
/// Resolves <see cref="ILazClient"/> instances by registration name. Use for multi-tenant scenarios
/// where one process holds multiple <see cref="LazClientOptions"/> (different app keys, regions, etc.).
/// </summary>
public interface ILazClientFactory
{
    /// <summary>Create (or return) the client registered under <paramref name="name"/>.</summary>
    ILazClient CreateClient(string name);
}
