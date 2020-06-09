using ForBrowser.Model;
using Microsoft.Extensions.Configuration;

namespace ForBrowser.Services
{
    public class ServerAssets
    {
        public ServerAssets(IConfiguration config)
        {
            var server = config["DemoServer"];
            var siteConfig = config.GetSection(nameof(SiteConfig))
                .Get<SiteConfig>();

            ClockImg = siteConfig.ForServerSite + "img/alarmClock.svg";
        }

        public readonly string ClockImg;
    }
}