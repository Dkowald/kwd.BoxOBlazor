using System;
using System.Collections.Generic;
using System.Linq;

using kwd.BoxOBlazor.Demo.Services.Clock;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace kwd.BoxOBlazor.Demo.Services.MemLog
{
    /// <summary>
    /// Simple memory logging.
    /// </summary>
    /// <remarks>
    /// todo: (re)consider if this should have ui events.
    /// doesn't feel like part of its responsibility.
    /// </remarks>
    [ProviderAlias(nameof(MemoryLogger))]
    public class MemoryLogger : ILoggerProvider
    {
        private readonly object _lock = new object();

        private readonly IClock _clock;
        private readonly List<LogEntry> _items;
        private readonly List<LoggingFilter> _filters;

        private readonly long _minInterLogDuration;
        private long _lastLogRecord;

        private readonly int _reserved;

        public static ILoggingBuilder Install(ILoggingBuilder loggingBuilder)
        {
            var clock = new DefaultClock();
            var provider = new MemoryLogger(clock);

            loggingBuilder.Services.AddSingleton(provider);

            return loggingBuilder.AddProvider(provider);
        }

        public MemoryLogger(IClock clock)
        {
            _lastLogRecord = clock.Now.Ticks;

            //no more than 1 per 50msec.
            _minInterLogDuration = TimeSpan.FromMilliseconds(50).Ticks;

            _filters = new List<LoggingFilter>
            {
                new LoggingFilter("Microsoft", LogLevel.Warning)
            };


            _clock = clock;
            _reserved = 10;
            var capacity = _reserved + _reserved / 2;
            //show last 10.. so keep list of 15.
            _items = new List<LogEntry>(capacity);
        }

        public void Dispose(){}

        public ILogger CreateLogger(string categoryName)
            => new Logger(this, categoryName);

        public void RemoveFilter(string name)
        {
            var found = _filters.FirstOrDefault(x => x.Prefix == name);

            if(found is null)return;
            
            _filters.RemoveAll(x => x.Prefix == name);
            FiltersUpdated?.Invoke();
        }

        public void AddFilter(string name, LogLevel level)
        {
            var found = _filters.FirstOrDefault(x => x.Prefix == name);

            if (found is null)
            {
                _filters.Add(new LoggingFilter(name, level));
                FiltersUpdated?.Invoke();
            }
        }

        public Action FiltersUpdated;

        public IEnumerable<LoggingFilter> ActiveFilters => _filters;

        public int Dropped;

        public IEnumerable<LogEntry> Items
        {
            get
            {
                lock (_lock)
                {
                    return _items.TakeLast(_reserved).Reverse();
                }
            }
        }

        public event Action ItemsUpdated;

        private void Insert(LogEntry entry)
        {
            if(_filters.Any() && !_filters.All(x => x.ShouldLogItem(entry)))
                return;

            if (_clock.Now.Ticks < _lastLogRecord + _minInterLogDuration)
            {
                Dropped++; return;
            }

            lock (_lock)
            {
                if (Dropped > 0)
                {
                    _items.Add(new LogEntry(_clock.Now, 
                        GetType().FullName, LogLevel.Critical,
                        0, null, null,
                        $"Logging overloaded: dropped {Dropped} items"));
                    Dropped = 0;
                }

                _items.Add(entry);

                if (_items.Count >= _reserved + _reserved / 2)
                    _items.RemoveRange(0, _reserved / 2);

                _lastLogRecord = _clock.Now.Ticks;
            }

            ItemsUpdated?.Invoke();
        }
        
        public class Logger : ILogger, IDisposable
        {
            private readonly MemoryLogger _owner;
            private readonly string _category;

            public Logger(MemoryLogger owner, string category)
            {
                _owner = owner;
                _category = category;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                var msg = formatter(state, exception);
                var item = new LogEntry(
                    _owner._clock.Now,
                    _category, logLevel, eventId, state, exception, msg);
                _owner.Insert(item);
            }

            public bool IsEnabled(LogLevel logLevel) => true;

            public IDisposable BeginScope<TState>(TState state)
                => new Logger(_owner, _category);

            public void Dispose() { }
        }
    }
}
