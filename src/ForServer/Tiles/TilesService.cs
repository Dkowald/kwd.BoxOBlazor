using Microsoft.AspNetCore.Http;

namespace ForServer.Tiles
{
    public class TilesService
    {
        public TilesService(string rootUrl)
        {
            RootUrl = rootUrl;

            RawRootUrl = rootUrl.Trim('/').ToLower();
        }

        public PathString RootUrl { get; private set; }

        public readonly string RawRootUrl;
    }
}