using System;
using System.Net.Http;
using System.Net.Http.Headers;
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
        /// <remarks>
        /// Since this is relative to the current site,
        /// MUST also be sure to allow bypass in service-worker.offline
        /// </remarks>
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
            var jsonType = new MediaTypeWithQualityHeaderValue("application/json");
            //host provided config.
            var jsonGet = new HttpClient
            {
                BaseAddress = client.BaseAddress
            };
            jsonGet.DefaultRequestHeaders.Accept
                .Add(jsonType);

            var cfgResp = await jsonGet.GetAsync(HostConfig);
            if (cfgResp.IsSuccessStatusCode)
            {
                var isJson = jsonType.Equals(cfgResp.Content.Headers.ContentType);
                
                if(isJson)
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
