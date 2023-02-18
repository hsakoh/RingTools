using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using RingFunctionAppHost;
using RingFunctionAppHost.Logics;
using System;
using System.Net;
using System.Net.Http;

[assembly: FunctionsStartup(typeof(Startup))]
namespace RingFunctionAppHost;

/// <summary>
/// FunctionStartup
/// </summary>

public class Startup : FunctionsStartup
{
    /// <inheritdoc/>
    public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
    {
    }

    /// <inheritdoc/>
    public override void Configure(IFunctionsHostBuilder builder)
    {
        WebProxy? debugProxy = null;
#if DEBUG
        debugProxy = new WebProxy("localhost", 8888);
#endif
        builder.Services.AddHttpClient<RingSession>()
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
            {
                Proxy = debugProxy
            })
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));
    }
}
