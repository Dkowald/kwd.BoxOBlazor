using System;

namespace ForServer.Services
{
    /// <summary>
    /// Application state model.
    /// </summary>
    public class AppState
    {
        public DateTime ServerTime;
        public TimeSpan UpTime;
        public event Action UpTimeChanged;

        public AppState(TimedCallback timedCallbacks, IClock sysClock)
        {
            var start = sysClock.Now;

            //Up time and clock.
            timedCallbacks.Register(
                () =>
                {
                    ServerTime = sysClock.Now;
                    UpTime = ServerTime - start;
                    OnUpTimeChanged();
                },
                TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }

        private void OnUpTimeChanged()
        {
            UpTimeChanged?.Invoke();
        }
    }
}
