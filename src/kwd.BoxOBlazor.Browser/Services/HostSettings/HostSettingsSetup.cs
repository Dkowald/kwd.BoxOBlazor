using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;

namespace kwd.BoxOBlazor.Browser.Services.HostSettings
{
    public static class HostSettingsSetup
    {
        /// <summary>
        /// endpoint for additional configuration from host service.
        /// </summary>
        /// <remarks>
        /// Since this is relative to the current site,
        /// MUST also be sure to allow bypass in service-worker.offline
        /// </remarks>
        public const string HostConfig = "HostConfig";

        /// <summary>
        /// Add 
        /// </summary>
        public static async Task AddHostConfiguration(this WebAssemblyHostBuilder builder, 
            HttpClient client, bool require = true)
        {
            var jsonType = new MediaTypeWithQualityHeaderValue("application/json");

            //host provided config.
            var jsonGet = new HttpClient
            {
                BaseAddress = client.BaseAddress
            };
            jsonGet.DefaultRequestHeaders.Accept
                .Add(jsonType);

            var cfgResp = await jsonGet.GetAsync(HostConfig);

            if (!cfgResp.IsSuccessStatusCode && require)
            { throw new HostSettingsError($"Request failed: status code {cfgResp.StatusCode}"); }

            if (cfgResp.IsSuccessStatusCode)
            {
                var mediaType = cfgResp.Content.Headers.ContentType?.MediaType;
                var isJson = string.Equals("application/json", mediaType, StringComparison.OrdinalIgnoreCase);
                if (!isJson && require)
                { throw new HostSettingsError($"Invalid content type from host: {cfgResp.Content.Headers.ContentType}"); }

                try
                {
                    builder.Configuration.AddJsonStream(await cfgResp.Content.ReadAsStreamAsync());
                }
                catch (JsonException ex)
                {
                    throw new HostSettingsError(ex);
                }
            }
        }
    }
}
