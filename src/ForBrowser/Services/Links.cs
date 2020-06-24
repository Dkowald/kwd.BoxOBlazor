using System;
using System.Collections.Generic;
using System.Linq;

using ForBrowser.Model;

namespace ForBrowser.Services
{
    public class Links
    {
        private readonly List<ExternalLink> _data;

        public Links()
        {
            _data = new List<ExternalLink>{
                new ExternalLink("nginx", "https://www.nginx.com/")
            };
        }

        /// <summary>
        /// Search for an external link
        /// </summary>
        public ExternalLink this[string name]
        {
            get
            {
                var found = 
                    _data
                        .FirstOrDefault(x => x.Text == name) ??

                   _data.FirstOrDefault(x => x.Text.Equals(name, StringComparison.OrdinalIgnoreCase)) ??
                    
                    _data.FirstOrDefault(x => x.Text.Contains(name, StringComparison.OrdinalIgnoreCase));

                return found;
            }
        }
    }
}