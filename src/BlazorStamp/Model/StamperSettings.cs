namespace BlazorStamp.Model
{
    public class StamperSettings
    {
        /// <summary>
        /// Physical path to wasm site files.
        /// </summary>
        public string PhysicalPath { get; set; }

        /// <summary>
        /// Url Base path for server; default file base element set to this.
        /// </summary>
        public string UrlBase { get; set; }

        /// <summary>
        /// Html start file.
        /// </summary>
        public string DefaultFile { get; set; } = "index.html";
    }
}