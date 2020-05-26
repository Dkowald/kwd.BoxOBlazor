using System;
using System.Net.Http;
using System.Threading.Tasks;

using ForBrowser.Model;
using ForBrowser.Services;

using kwd.BoxOBlazor;
using kwd.BoxOBlazor.Services;
using kwd.BoxOBlazor.Services.Logging;
using kwd.BoxOBlazor.Web.scripts.util;
using kwd.BoxOBlazor.Web.util;
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

            var memLog = new MemoryLogger(new DefaultClock());
            builder.Services.AddSingleton(memLog);
            builder.Logging
                .SetMinimumLevel(LogLevel.Information)
                .AddProvider(memLog);

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
