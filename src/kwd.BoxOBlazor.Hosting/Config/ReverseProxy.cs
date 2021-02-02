using System;
using System.Linq;

namespace kwd.BoxOBlazor.Hosting.Config
{
    public class ReverseProxy
    {
        private string[] _allowedHosts = Array.Empty<string>();

        /// <summary>
        /// List of Allowed hosts
        /// </summary>
        public string[] AllowedHosts
        {
            get => _allowedHosts;
            set
            {
                _allowedHosts =
                    value.SelectMany(x => x.Split(';'))
                        .Select(x => x.Trim())
                        .Where(x => !string.IsNullOrEmpty(x))
                        .ToArray();
            }
        }
        /// <summary>
        /// Semi-collin separated set of
        /// reverse proxy's permitted to provide X-Forward headers.
        /// </summary>
        public string[] Addresses { get; set; } = Array.Empty<string>();
    }
}
