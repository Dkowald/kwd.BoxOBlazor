using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace kwd.BoxOBlazor.Demo.Services.Time
{
    /// <summary>
    /// Timer based callbacks.
    /// </summary>
    public class UITimers : IUITimers
    {
        private readonly Dictionary<string, Timer> _timers;

        /// <summary>
        /// Create new <see cref="UITimers"/> service.
        /// </summary>
        public UITimers()
        {
            _timers = new Dictionary<string, Timer>();
        }

        /// <inheritdoc/>
        public ValueTask<string> Register(Action op,
            TimeSpan startDelay,
            TimeSpan repeatPeriod,
            string name = null)
        {
            var item = new Timer(_ => op(), null, startDelay, repeatPeriod);

            name ??= $"Alarm-{_timers.Count + 1}";

            _timers.Add(name, item);

            return new ValueTask<string>(name);
        }

        /// <inheritdoc/>
        public async ValueTask<bool> UnRegister(string name)
        {
            if (_timers.TryGetValue(name, out var item))
            {
                await item.DisposeAsync();
                return true;
            }

            return false;
        }
    }
}