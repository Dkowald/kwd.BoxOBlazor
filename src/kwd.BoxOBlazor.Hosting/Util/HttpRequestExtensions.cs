using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace kwd.BoxOBlazor.Hosting.Util
{
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// The base url from the request.
        /// Uses <see cref="HttpRequest.PathBase"/> to
        /// determine the base url of the request.
        /// Base url ALWAYS ends with a /
        /// </summary>
        public static string GetBaseURL(this HttpRequest request)
        {
            //var basePath = request.PathBase.HasValue ? request.PathBase.Value.TrimEnd('/') : "";
            //var url = $"{request.Scheme}://{request.Host}{basePath}/";
            //return new Uri(url, UriKind.Absolute);

            var url = UriHelper.BuildAbsolute(
                request.Scheme,
                request.Host,
                request.PathBase);

            url = url.EndsWith('/') ? url : url + '/';
            return url;
        }
    }
}