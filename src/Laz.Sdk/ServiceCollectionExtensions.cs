using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Laz.Sdk;

/// <summary>
/// <see cref="IServiceCollection"/> extensions for registering <see cref="ILazClient"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>Register the default <see cref="ILazClient"/> with inline configuration.</summary>
    public static IHttpClientBuilder AddLazClient(
        this IServiceCollection services,
        Action<LazClientOptions> configure)
        => services.AddLazClientCore(Options.DefaultName, configure, registerDefault: true);

    /// <summary>Register the default <see cref="ILazClient"/> bound from <see cref="IConfiguration"/>.</summary>
    public static IHttpClientBuilder AddLazClient(
        this IServiceCollection services,
        IConfiguration configuration)
        => services.AddLazClientCore(Options.DefaultName, configuration, registerDefault: true);

    /// <summary>Register a named client resolvable via <see cref="ILazClientFactory.CreateClient"/>.</summary>
    public static IHttpClientBuilder AddLazClient(
        this IServiceCollection services,
        string name,
        Action<LazClientOptions> configure)
        => services.AddLazClientCore(name, configure, registerDefault: false);

    /// <summary>Register a named client (bound from configuration) resolvable via <see cref="ILazClientFactory.CreateClient"/>.</summary>
    public static IHttpClientBuilder AddLazClient(
        this IServiceCollection services,
        string name,
        IConfiguration configuration)
        => services.AddLazClientCore(name, configuration, registerDefault: false);

    private static IHttpClientBuilder AddLazClientCore(
        this IServiceCollection services,
        string name,
        Action<LazClientOptions> configure,
        bool registerDefault)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(configure);

        services.AddOptions<LazClientOptions>(name)
                .Configure(configure)
                .ValidateDataAnnotations()
                .ValidateOnStart();

        return services.WireUpNamed(name, registerDefault);
    }

    private static IHttpClientBuilder AddLazClientCore(
        this IServiceCollection services,
        string name,
        IConfiguration configuration,
        bool registerDefault)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddOptions<LazClientOptions>(name)
                .Bind(configuration)
                .ValidateDataAnnotations()
                .ValidateOnStart();

        return services.WireUpNamed(name, registerDefault);
    }

    private static IHttpClientBuilder WireUpNamed(
        this IServiceCollection services,
        string name,
        bool registerDefault)
    {
        services.TryAddSingleton<ILazClientFactory, LazClientFactory>();

        var clientName = $"Laz.Sdk:{name}";
        var builder = services.AddHttpClient(clientName, (sp, http) =>
        {
            var opts = sp.GetRequiredService<IOptionsMonitor<LazClientOptions>>().Get(name);
            http.Timeout = opts.Timeout;
        });

        if (registerDefault)
        {
            services.TryAddTransient<ILazClient>(sp =>
            {
                var http = sp.GetRequiredService<IHttpClientFactory>().CreateClient(clientName);
                var opts = sp.GetRequiredService<IOptionsMonitor<LazClientOptions>>().Get(name);
                return new LazClient(http, opts);
            });
        }

        return builder;
    }
}
