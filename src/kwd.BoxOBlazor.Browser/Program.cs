using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using kwd.BoxOBlazor.Browser.Model;
using kwd.BoxOBlazor.Browser.Services;
using kwd.BoxOBlazor.Demo.Model;
using kwd.BoxOBlazor.Demo.Services.MemLog;
using kwd.BoxOBlazor.Demo.Services.Time;
using kwd.BoxOBlazor.Demo.util;
using kwd.BoxOBlazor.Demo.util.ClientSideStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace kwd.BoxOBlazor.Browser
{
    public static class Program
    {
        /// <summary>
        /// endpoint for additional configuration from host service.
        /// </summary>
        public const string HostConfig = "HostConfig";

		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("app");

            var http = new HttpClient
            {
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            };
            
            builder.Services.AddSingleton(http);

            await AddHostConfiguration(builder, http);

            builder.Logging
                .SetMinimumLevel(LogLevel.Information)
                .AddMemoryLogger();

            ConfigureServices(builder.Services);
            
			await builder.Build()
                .RunAsync();
		}

		static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IClock, DefaultClock>();

            services.AddSingleton<Links>();

            services.AddScoped<AppState>();

            services.AddSingleton<IUITimers, UITimers>();

            SiteConfig.Register(services);

            //browser services.
            services
                .AddScoped<Prompt>()
                .AddScoped<Clipboard>();

            services.AddScoped<LocalStorage>();
        }

        static async Task AddHostConfiguration(WebAssemblyHostBuilder builder, HttpClient client)
        {
            //host provided config.
            var cfgResp = await client.GetAsync(HostConfig);
            if (cfgResp.IsSuccessStatusCode)
            {
                try
                {
                    builder.Configuration.AddJsonStream(await cfgResp.Content.ReadAsStreamAsync());
                }
                catch (JsonException ex)
                {
                    throw new ApplicationException("Error reading host config", ex);
                }
            }
        }
    }
}
