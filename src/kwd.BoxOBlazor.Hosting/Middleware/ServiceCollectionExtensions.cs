using Microsoft.Extensions.DependencyInjection;

namespace kwd.BoxOBlazor.Hosting.Middleware
{
    /// <summary>
    /// Extensions to help register with application IOC.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register <see cref="ForwardBasePath"/> with container.
        /// </summary>
        public static IServiceCollection AddForwardBasePath(this IServiceCollection services)
        {
            services.AddScoped<ForwardBasePath>();

            return services;
        }
    }
}