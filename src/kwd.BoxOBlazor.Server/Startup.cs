using kwd.BoxOBlazor.Demo.Model;
using kwd.BoxOBlazor.Demo.Services.MemLog;
using kwd.BoxOBlazor.Demo.Services.Time;
using kwd.BoxOBlazor.Demo.util;
using kwd.BoxOBlazor.Demo.util.ClientSideStorage;
using kwd.BoxOBlazor.Server.Config;
using kwd.BoxOBlazor.Server.Data;
using kwd.BoxOBlazor.Server.Tiles;
using kwd.WebCore.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace kwd.BoxOBlazor.Server
{
    public class Startup
	{
		private readonly IConfiguration _configuration;

		public Startup(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		
		public void ConfigureServices(IServiceCollection services)
        {
            services.AddReverseProxy(_configuration);

            services.AddLogging(cfg =>
                {
                    cfg.AddMemoryLogger();

					cfg.AddFile(
                        _configuration.GetSection("Logging"));
            });

            services.AddOptions()
                .Configure<SiteConfig>(
                    _configuration.GetSection(nameof(SiteConfig)))
                .Configure<CircuitOptions>(
                    _configuration.GetSection(nameof(CircuitOptions)));
            
            //server timing events
            services.AddSingleton<IUITimers, UITimers>()
                .AddSingleton<IClock, DefaultClock>();

			services.AddControllersWithViews();
			services.AddRazorPages();

            services.AddServerSideBlazor()
                .AddCircuitOptions(cfg => cfg.DetailedErrors=true);
            
			services.AddSingleton<WeatherForecastService>();

            services.AddSingleton<AppState>();

            services.AddSingleton(new TilesService("./"));

            //browser services
            services
                .AddScoped<Prompt>()
                .AddScoped<Clipboard>();

            services.AddScoped<LocalStorage>();
        }

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseReverseProxy();
            
            if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
            }

			app.UseHttpsRedirection();

            app.UseDefaultFiles()
                .UseStaticFiles();

            app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapDefaultControllerRoute();
				endpoints.MapRazorPages();

				endpoints.MapBlazorHub();

				//Map all other URL's to this for a 404 display.
				endpoints.MapFallbackToPage("/_Host");
			});
		}

    }
}
