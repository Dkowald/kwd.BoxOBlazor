using System.Threading.Tasks;
using ForBrowser.Model;
using ForBrowser.Services;
using kwd.BoxOBlazor;
using kwd.BoxOBlazor.Services;
using kwd.BoxOBlazor.Services.Logging;

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
			
			ConfigureServices(builder.Services);

			await builder.Build().RunAsync();
		}

		static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IClock, DefaultClock>()
                .AddSingleton<ServerAssets>();

            services.AddScoped<AppState>();
            services.AddSingleton<UITimer>();

            //todo: can i use option pattern for config data?
            
            services.AddLogging(cfg =>
            {
                //console logging. 
                // (console mapped to browser console)
                cfg.AddFilter(null, LogLevel.Information)
                    .AddConsole();

                var provider = new MemoryLogger(new DefaultClock());
                cfg.Services.AddSingleton(provider);
                cfg.AddProvider(provider);
            });
	
			services.AddBaseAddressHttpClient();

			services.AddScoped<JsProxy>();
        }
    }
}
