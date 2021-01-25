using kwd.BoxOBlazor.Browser.Model;
using kwd.BoxOBlazor.Hosting.Config;
using kwd.BoxOBlazor.Hosting.Middleware;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace kwd.BoxOBlazor.Browser.Host
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            new SetupReversProxy().ConfigureServices(services, _config);

            services.Configure<SiteConfig>(_config.GetSection(nameof(SiteConfig)));

            services.AddRazorPages();

            services.AddForwardBasePath();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            new SetupReversProxy().Configure(app);
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
                
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
