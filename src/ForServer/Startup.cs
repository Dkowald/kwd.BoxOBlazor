using System.Linq;

using ForServer.Data;
using ForServer.Services;

using kwd.BoxOBlazor;
using kwd.BoxOBlazor.Services;
using kwd.BoxOBlazor.Services.Logging;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ForServer
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
            services
                .AddLogging(cfg =>
                {
					var provider = new MemoryLogger(new DefaultClock());
					
                    cfg.Services.AddSingleton(provider);

                    cfg.AddProvider(provider);
                    cfg.AddFile(
                        _configuration.GetSection("Logging"));
            });

			//server timing events
            services.AddSingleton<TimedCallback>()
                .AddHostedService(ctx => ctx.GetRequiredService<TimedCallback>());

            services.AddSingleton<IClock, DefaultClock>();

			services.AddControllersWithViews();
			services.AddRazorPages();

			services.AddServerSideBlazor();

			services.AddSingleton<WeatherForecastService>();

            services.AddSingleton<AppState>();
			
			services.AddScoped<JsProxy>();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });
            app.Use((context, next) =>
            {
                if (context.Request.Headers.TryGetValue("X-Forwarded-PathBase", out var pathBases))
                {
                    context.Request.PathBase = pathBases.First();
                }
                return next();
            });

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			
			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapDefaultControllerRoute();
				endpoints.MapRazorPages();

				endpoints.MapBlazorHub();
				endpoints.MapFallbackToPage("/_Host");
			});
		}
	}
}
