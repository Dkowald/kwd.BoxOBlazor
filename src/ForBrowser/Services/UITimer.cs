using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ForBrowser.Services
{
    public class UITimer
    {
        private readonly Dictionary<string, Timer> _timers;

        public UITimer()
        {
            _timers = new Dictionary<string, Timer>();
        }

        public string Register(Action op,
            TimeSpan startDelay,
            TimeSpan repeatPeriod,
            string name = null)
        {
            var item = new Timer(_ => op(), null, startDelay, repeatPeriod);

            name ??= $"Alarm-{_timers.Count + 1}";

            _timers.Add(name, item);

            return name;
        }

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
