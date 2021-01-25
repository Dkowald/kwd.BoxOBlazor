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
    public class SetupReversProxy
    {
        public void Configure(IApplicationBuilder app)
        {
            Configure(app,
                app.ApplicationServices.GetRequiredService<IOptions<ReverseProxy>>(),
                app.ApplicationServices.GetRequiredService<ILogger<SetupReversProxy>>()
                );
        }
        
        public void ConfigureServices(IServiceCollection services, IConfiguration config)
        {
            services.Configure<ReverseProxy>(config.GetSection(nameof(ReverseProxy)));
        }

        public void Configure(IApplicationBuilder app,
            IOptions<ReverseProxy> proxyOptions,
            ILogger log)
        {
            var cfg = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            };

            var proxyConfig = proxyOptions.Value;
            if (proxyConfig.Addresses.Any())
            {
                log.LogInformation("Configuring accepted reverse proxies");
                
                //reset any existing
                cfg.KnownNetworks.Clear();
                cfg.KnownProxies.Clear();

                foreach (var address in proxyConfig.Addresses)
                {
                    var ip = IPAddress.Parse(address);
                    log.LogDebug("Adding reverse proxy: {reverseProxy}", ip.ToString());
                    cfg.KnownProxies.Add(ip);
                }
            }

            app.UseForwardedHeaders(cfg);

            app.UseForwardBasePath();
        }
    }
}