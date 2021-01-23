using System.Linq;
using System.Threading.Tasks;
using kwd.CoreUtil.Strings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace kwd.BoxOBlazor.Hosting.Middleware
{
    /// <summary>
    /// Update <see cref="HttpRequest.PathBase"/> to reflect
    /// front proxy provided Header, <see cref="ForwardBasePathOptions.HeaderKey"/>.
    /// </summary>
    /// <remarks>
    /// todo: this is part of kwd.WebCore, move to that once have package for it.
    /// Base path will be empty, or a PathString
    /// If provided; it will NOT end in a /
    /// <see cref="https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.httprequest.pathbase?view=aspnetcore-5.0"/>
    /// </remarks>
    public class ForwardBasePath : IMiddleware
    {
        private readonly ILogger<ForwardBasePath> _log;
        private readonly ForwardBasePathOptions _options;
        
        /// <summary>
        /// Create a new <see cref="ForwardBasePath"/>.
        /// </summary>
        public ForwardBasePath(IOptions<ForwardBasePathOptions> options, ILogger<ForwardBasePath> log)
        {
            _options = options.Value;
            _log = log;
        }

        /// <inheritdoc cref="IMiddleware.InvokeAsync"/>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {   
            //last one wins.
            var header = context.Request.Headers.Where(x => x.Key.Same(_options.HeaderKey)).ToArray();
            if (header.Any())
            {
                var baseValue = header.SelectMany(x => x.Value)
                    .Last(x => !string.IsNullOrWhiteSpace(x))
                    .TrimEnd('/');

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
