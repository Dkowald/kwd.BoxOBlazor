using Microsoft.Extensions.Logging;

namespace ForServer.Services.Logging
{
    public class LoggingFilter
    {
        public LoggingFilter(string prefix, LogLevel levelOrMore)
        {
            Prefix = prefix;
            Level = levelOrMore;
        }

        public readonly string Prefix;
        public readonly LogLevel Level;

        public bool ShouldLogItem(LogEntry entry)
            => !entry.Category.StartsWith(Prefix) ||
               entry.Level >= Level && Level != LogLevel.None;
    }
}