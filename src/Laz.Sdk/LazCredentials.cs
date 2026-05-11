namespace Laz.Sdk;

/// <summary>
/// Per-call credential override returned by <see cref="ILazClient.WithCredentials"/>.
/// Use when <see cref="LazClientOptions.AppKey"/> / <see cref="LazClientOptions.AppSecret"/>
/// are not known at DI registration time (e.g. multi-tenant SaaS pulling tenant creds
/// from a database at request time).
/// </summary>
/// <param name="AppKey">Tenant app key.</param>
/// <param name="AppSecret">Tenant app secret used for HMAC-SHA256 signing.</param>
/// <param name="ServerUrl">Optional regional gateway. Falls back to <see cref="LazClientOptions.ServerUrl"/> when null.</param>
public sealed record LazCredentials(string AppKey, string AppSecret, string? ServerUrl = null);
