using Microsoft.Extensions.Configuration;

namespace ForBrowser.Model
{
    public class ServerAssets
    {
        public ServerAssets(IConfiguration config)
        {
            var server = config["DemoServer"];

            ClockImg = server + "img/alarmClock.svg";
        }

        public readonly string ClockImg;
    }
}