using System;

namespace ForServer.Services
{
    /// <summary>
    /// Application state model.
    /// </summary>
    public class AppState
    {
        private readonly DateTime _start;

        public DateTime ServerTime;
        public TimeSpan UpTime;

        public event Action UpTimeChanged;

        public AppState(TimedCallback alarmClock, IClock sysClock)
        {
            _start = sysClock.Now;

            //Up time and clock.
            alarmClock.Register(
                () =>
                {
                    ServerTime = sysClock.Now;
                    UpTime = ServerTime - _start;
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
