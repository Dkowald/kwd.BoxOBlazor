using System;
using Microsoft.Extensions.Logging;

namespace kwd.BoxOBlazor.Demo.Services.MemLog
{
    public class LogEntry
    {
        public LogEntry(
            DateTime when,
            string category,
            LogLevel logLevel,
            EventId eventId,
            object state,
            Exception exception,
            string message)
        {
            When = when;
            Category = category;
            Level = logLevel;
            Id = eventId;
            State = state;
            Exception = exception;
            Message = message;
        }

        public readonly DateTime When;
        public readonly string Category;
        public readonly LogLevel Level;
        public readonly EventId Id;
        public readonly object State;
        public readonly Exception Exception;
        public readonly string Message;
    }
}