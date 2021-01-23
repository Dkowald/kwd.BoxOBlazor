namespace kwd.BoxOBlazor.Server.Tiles
{
    public class TilesService
    {
        public TilesService(string rootUrl)
        {
            RootUrl = rootUrl;
        }

        public string RootUrl { get; private set; }
    }
}