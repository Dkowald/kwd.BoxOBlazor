namespace kwd.BoxOBlazor.Hosting.Middleware
{
    /// <summary>
    /// Options for the <see cref="ForwardBasePath"/> middleware.
    /// </summary>
    public class ForwardBasePathOptions
    {
        /// <summary>
        /// Default header from up-stream proxy.
        /// </summary>
        public const string DefaultHeaderKey = "X-Forwarded-Prefix";

        /// <summary>
        /// Create a new <see cref="ForwardBasePathOptions"/>.
        /// </summary>
        public ForwardBasePathOptions()
        {
            HeaderKey = DefaultHeaderKey;
        }

        /// <summary>
        /// Http header key used to identify sub-path.
        /// </summary>
        public string HeaderKey { get; set; }
    }
}