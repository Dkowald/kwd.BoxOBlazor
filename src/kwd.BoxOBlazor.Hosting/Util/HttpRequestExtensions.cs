using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace kwd.BoxOBlazor.Hosting.Util
{
    /// <summary>
    /// Extensions for <see cref="HttpRequest"/>
    /// </summary>
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// The base url from the request.
        /// Uses <see cref="HttpRequest.PathBase"/> to
        /// determine the base url of the request.
        /// Base url has trailing /
        /// </summary>
        /// <remarks>
        /// The <see cref="HttpRequest.PathBase"/> is assumed to
        /// not end in trailing / (as per docs).
        /// 
        /// Standardizing to have a trailing /
        /// </remarks>
        public static string GetBaseURL(this HttpRequest request)
        {
            var url = UriHelper.BuildAbsolute(
                request.Scheme,
                request.Host,
                request.PathBase);

            url = url.EndsWith('/') ? url : url + '/';
            
            return url;
        }
    }
}