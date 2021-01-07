using System.Linq;
using System.Threading.Tasks;
using kwd.CoreUtil.Strings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ForBrowser.Host.Middleware
{
    /// <summary>
    /// Middleware to set base path using re-verse-proxy header.
    /// </summary>
    /// <remarks>
    /// todo: move this into a shared package.
    /// </remarks>
    public class ForwardPrefix : IMiddleware
    {
        public const string HeaderKey = "X-Forwarded-Prefix";

        private readonly ILogger<ForwardPrefix> _log;

        public ForwardPrefix(ILogger<ForwardPrefix> log)
        {
            _log = log;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            //last one wins.
            var header = context.Request.Headers.Where(x => x.Key.Same(HeaderKey)).ToArray();
            if (header.Any())
            {
                var baseValue = header.SelectMany(x => x.Value)
                    .Last().Trim().TrimEnd('/');

                if (baseValue != string.Empty)
                {
                    if (baseValue.Length > 1 && baseValue[0] != '/')
                        baseValue = "/" + baseValue;

                    _log.LogTrace("PathBase set using forward-header value '{forward-prefix}'", baseValue);
                    context.Request.PathBase = new PathString(baseValue);
                }
            }

            await next(context);
        }
    }
}
