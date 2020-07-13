namespace ForServer.Tiles
{
    /// <summary>
    /// A tile described by a basic class, rather than a blazor component
    /// </summary>
    public class BasicTile
    {
        private const int DescriptionMaxChar = 115;

        /// <summary>
        /// Short one-line text title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// site relative icon path
        /// </summary>
        public string IconUrl { get; set; }
        
        /// <summary>
        /// Short para description.
        /// Length limited to fit content area : 132
        /// </summary>
        public string Description
        {
            get => _description;
            set =>
                //todo: break on word; so ellipse better
                _description = value.Length > DescriptionMaxChar ? value.Substring(0,  DescriptionMaxChar-1) + "..." : value;
        }

        public string OpenUrl { get; set; }

        private string _description;
    }
}