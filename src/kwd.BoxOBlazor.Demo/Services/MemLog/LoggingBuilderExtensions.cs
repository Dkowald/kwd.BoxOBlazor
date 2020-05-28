using Microsoft.Extensions.Logging;

namespace kwd.BoxOBlazor.Demo.Services.MemLog
{
    public static class LoggingBuilderExtensions
    {
        public static ILoggingBuilder AddMemoryLogger(this ILoggingBuilder builder)
            => MemoryLogger.Install(builder);
    }
}