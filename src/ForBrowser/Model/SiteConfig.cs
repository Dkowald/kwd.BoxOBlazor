using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ForBrowser.Model
{
    public class SiteConfig
    {
        /// <summary>
        /// Register access to config object.
        /// </summary>
        /// <remarks>
        /// todo: consider IOption pattern in future.
        /// </remarks>
        public static IServiceCollection Register(IServiceCollection svc)
        {
            svc.AddScoped<SiteConfig>(ctx =>
            {
                var cfg = ctx.GetRequiredService<IConfiguration>();

                return cfg.GetSection(nameof(SiteConfig))
                    .Get<SiteConfig>();
            });
            return svc;
        }

        public string ForServerSite { get; set; }
    }
}