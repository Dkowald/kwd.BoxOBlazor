using System;
using System.Net.Http;
using System.Threading.Tasks;
using kwd.BoxOBlazor.Browser.Model;
using kwd.BoxOBlazor.Browser.Services;
using kwd.BoxOBlazor.Demo.Model;
using kwd.BoxOBlazor.Demo.Services.MemLog;
using kwd.BoxOBlazor.Demo.Services.Time;
using kwd.BoxOBlazor.Demo.util;
using kwd.BoxOBlazor.Demo.util.ClientSideStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace kwd.BoxOBlazor.Browser
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
    }
}
