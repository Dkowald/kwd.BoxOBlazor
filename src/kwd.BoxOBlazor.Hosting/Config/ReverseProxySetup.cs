using System;
using System.Linq;
using System.Net;

using kwd.BoxOBlazor.Hosting.Middleware;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace kwd.BoxOBlazor.Hosting.Config
{
    public static class ReverseProxySetup
    {
        /// <summary>
        /// Register to load and configure settings for reverse proxy headers.
        /// </summary>
        public static IServiceCollection AddReverseProxy(this IServiceCollection services,
            IConfiguration config)
        {
            services.Configure<ReverseProxy>(config.GetSection(nameof(ReverseProxy)));

            services.AddOptions<ForwardedHeadersOptions>()
                .Configure<IOptions<ReverseProxy>, ILogger<ReverseProxy>>(ConfigureOptions);

            services.AddScoped<ForwardBasePath>();

            return services;
        }

        /// <summary>
        /// Use reverse proxy middleware item(s)
        /// </summary>
        public static void UseReverseProxy(this IApplicationBuilder app)
        {
            EnsureServices(app.ApplicationServices);

            app.UseForwardedHeaders();
            app.UseMiddleware<ForwardBasePath>();
        }

        private static void EnsureServices(IServiceProvider services)
        {
            using var testScope = services.CreateScope();
            var marker = testScope.ServiceProvider.GetService<ForwardBasePath>();
            if (marker is null)
            {
                throw new InvalidOperationException(
                    "Reverse proxy services not registered," +
                    $"Include a call to {nameof(AddReverseProxy)}");
            }
        }

        private static void ConfigureOptions(
            ForwardedHeadersOptions cfg,
            IOptions<ReverseProxy> proxyOptions, ILogger<ReverseProxy> log)
        {
            var proxy = proxyOptions.Value;

            if (!proxy.Addresses.Any() && !proxy.AllowedHosts.Any())
            { return; }

            log.LogInformation("Configuring to accept reverse proxy headers");

            cfg.ForwardedHeaders = ForwardedHeaders.All;
            cfg.KnownProxies.Clear();
            cfg.KnownNetworks.Clear();
            cfg.AllowedHosts.Clear();
            cfg.ForwardLimit = 1;

            if (proxy.Addresses.Any())
            {
                foreach (var address in proxy.Addresses)
                {
                    var ip = IPAddress.Parse(address);
                    log.LogDebug("Adding reverse proxy: {reverseProxy}", ip.ToString());
                    cfg.KnownProxies.Add(ip);
                }
            }

            if (proxy.AllowedHosts.Any())
            {
                foreach (var host in cfg.AllowedHosts)
                {
                    log.LogDebug("Adding host: {host}", host);

                    cfg.AllowedHosts.Add(host);
                }
            }
        }
    }
}