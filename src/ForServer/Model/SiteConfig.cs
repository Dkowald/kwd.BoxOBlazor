namespace ForServer.Model
{
    public class SiteConfig
    {
        /// <summary>
        /// Set this to a local path for Browser site.
        /// </summary>
        public string WasmFileRoot { get; set; }

        /// <summary>
        /// Set this to a url for the WASM version.
        /// </summary>
        public string ForBrowserSite { get; set; }
    }
}