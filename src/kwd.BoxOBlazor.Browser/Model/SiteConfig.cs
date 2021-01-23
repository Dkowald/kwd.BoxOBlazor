using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace kwd.BoxOBlazor.Browser.Model
{
    public class SiteConfig
    {
        /// <summary>
        /// Site configuration.
        /// </summary>
        /// <remarks>
        /// todo: consider IOption pattern in future.
        /// </remarks>
        public static IServiceCollection Register(IServiceCollection svc)
        {
            svc.AddScoped(ctx =>
            {
                var cfg = ctx.GetRequiredService<IConfiguration>();

                return cfg.GetSection(nameof(SiteConfig))
                    .Get<SiteConfig>();
            });
            return svc;
        }

        public string ServerURL { get; set; }
    }
}