using Microsoft.AspNetCore.Builder;

namespace kwd.BoxOBlazor.Hosting.Middleware
{
    /// <summary>
    /// Extensions to help register with Application pipeline.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Add the <see cref="ForwardBasePath"/> middleware to the app services.
        /// </summary>
        public static IApplicationBuilder UseForwardBasePath(this IApplicationBuilder app)
        {
            app.UseMiddleware<ForwardBasePath>();

            return app;
        }
    }
}