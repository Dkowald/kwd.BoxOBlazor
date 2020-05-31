using System;
using System.Net.Http;
using System.Threading.Tasks;

using ForBrowser.Model;
using ForBrowser.Services;

using kwd.BoxOBlazor.Demo;
using kwd.BoxOBlazor.Demo.Services.Clock;
using kwd.BoxOBlazor.Demo.Services.MemLog;
using kwd.BoxOBlazor.Demo.util;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ForBrowser
{
    public static class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("app");

            builder.Services.AddSingleton(new HttpClient
            {
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            });

            builder.Logging
                .SetMinimumLevel(LogLevel.Information)
                .AddMemoryLogger();

            ConfigureServices(builder.Services);
            
			await builder.Build()
                .RunAsync();
		}

		static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IClock, DefaultClock>()
                .AddSingleton<ServerAssets>();

            services.AddScoped<AppState>();

            services.AddSingleton<UITimer>();

            SiteConfig.Register(services);

            services.AddScoped<JsProxy>();

            services.AddScoped<LocalStorage>();
        }
    }
}
