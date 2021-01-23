using System;

namespace kwd.BoxOBlazor.Hosting.Config
{
    public class ReverseProxy
    {
        /// <summary>
        /// Semi-collin separated set of
        /// reverse proxy's permitted to provide X-Forward headers.
        /// </summary>
        public string[] Addresses { get; set; } = Array.Empty<string>();
    }
}
