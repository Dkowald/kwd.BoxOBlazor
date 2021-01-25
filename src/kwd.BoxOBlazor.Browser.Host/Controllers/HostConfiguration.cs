using kwd.BoxOBlazor.Browser.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace kwd.BoxOBlazor.Browser.Host.Controllers
{
    /// <summary>
    /// Provide server-side config data.
    /// </summary>
    /// <remarks>
    /// Blazor WASM creates a browser-app, packaging the standard
    /// appsettings.json and including it in the browser-side
    /// assets.
    ///
    /// This adds a well-known endpoint to the hosting service, to
    /// offer up additional host generated config value(s).
    ///
    /// Rather than serving up the appsettings.json, use the server side configuration
    /// system to get the actual config value(s). This not only keeps the rich
    /// configuration ability on the server, but also uses code to limit the
    /// actual config data to only the bits for the browser app.
    /// </remarks>
    [ApiController]
    [Route(Browser.Program.HostConfig)]
    public class HostConfiguration : ControllerBase
    {
        private readonly IOptions<SiteConfig> _config;
        private readonly ILogger<HostConfiguration> _log;

        public HostConfiguration(IOptions<SiteConfig> config, ILogger<HostConfiguration> log)
        {
            _config = config;
            _log = log;
        }

        [HttpGet]
        public JsonResult Get()
        {
            _log.LogDebug("Browser app requested host configuration");

            var result = new Config
            {
                SiteConfig =
                {
                    ServerURL = _config.Value.ServerURL
                }
            };
            
            _log.LogInformation("ServerURL: {serverUrl}", result.SiteConfig.ServerURL);

            return new JsonResult(result);
        }

        public class Config
        {
            public SiteConfig SiteConfig { get; set; } = new SiteConfig();
        }
    }
}
