using System.IO;
using System.Linq;

using ForServer.Data;
using ForServer.Model;
using ForServer.Tiles;
using kwd.BoxOBlazor.Demo.Model;
using kwd.BoxOBlazor.Demo.Services.MemLog;
using kwd.BoxOBlazor.Demo.Services.Time;
using kwd.BoxOBlazor.Demo.util;
using kwd.BoxOBlazor.Demo.util.ClientSideStorage;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
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

            services.AddSingleton(new TilesService("/"));

            //browser services
            services
                .AddScoped<Prompt>()
                .AddScoped<Clipboard>();

            services.AddScoped<LocalStorage>();
        }

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ConfigureProxySupport(app);

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

            ConfigureLocalWASM(app);

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

        private void ConfigureProxySupport(IApplicationBuilder app)
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
        }

        private void ConfigureLocalWASM(IApplicationBuilder app)
        {
            var siteConfig = _configuration.GetSection(nameof(SiteConfig))
                .Get<SiteConfig>();

            if(string.IsNullOrWhiteSpace(siteConfig.WasmFileRoot)) return;
            
            //extra file types for blazor wasm support
            var mimeTypes = new FileExtensionContentTypeProvider();
            mimeTypes.Mappings[".dll"] = "application/octet-stream";
            mimeTypes.Mappings[".dat"] = "application/octet-stream";

            //if happen to be serving wasm from debug build.
            mimeTypes.Mappings[".pdb"] = "application/octet-stream";

            var wasmFiles = Path.Combine(
                Directory.GetCurrentDirectory(), siteConfig.WasmFileRoot);

            //serve up wasm static assets.
            app.UseFileServer(new FileServerOptions
            {
                EnableDefaultFiles = true,
                RequestPath = "/wasm",
                StaticFileOptions =
                {
                    FileProvider = new PhysicalFileProvider(wasmFiles),
                    ContentTypeProvider = mimeTypes
                }
            });
        }
    }
}
