using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ForServer.Services
{
    /// <summary>
    /// A service to call-back at some time.
    /// </summary>
    public class TimedCallback : IHostedService, IDisposable, IAsyncDisposable
    {
        private readonly Dictionary<string, Timer> _timers;

        public TimedCallback()
        {
            _timers = new Dictionary<string, Timer>();
        }

        /// <summary>
        /// Register a callback to be fired.
        /// This is 
        /// </summary>
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

        /// <summary>Remove existing alarm; return true if item removed.</summary>
        public async ValueTask<bool> UnRegister(string name)
        {
            if (_timers.TryGetValue(name, out var item))
            {
                await item.DisposeAsync();
                return true;
            }

            return false;
        }

        #region IHostedService
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (var item in _timers)
            {
                item.Value.Change(Timeout.Infinite, 0);
            }

            return Task.CompletedTask;
        }
        #endregion

        #region Disposable
        public void Dispose()
        {
            foreach (var item in _timers)
            {
                item.Value.Dispose();
            }
            _timers.Clear();
        }

        public async ValueTask DisposeAsync()
        {
            foreach (var item in _timers)
            {
                await item.Value.DisposeAsync();
            }
            _timers.Clear();
        }
        #endregion
    }
}