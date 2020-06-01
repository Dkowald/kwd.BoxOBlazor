using System;
using kwd.BoxOBlazor.Demo.Services.Time;

namespace kwd.BoxOBlazor.Demo.Model
{
    /// <summary>
    /// Simple app state
    /// </summary>
    public class AppState
    {   
        public AppState(IUITimers uiTimers, IClock sysClock)
        {
            var start = sysClock.Now;

            //Up time and clock.
            uiTimers.Register(
                () =>
                {
                    ServerTime = sysClock.Now;
                    UpTime = ServerTime - start;
                    OnUpTimeChanged();
                },
                TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }

        public DateTime ServerTime;
        public TimeSpan UpTime;
        public event Action UpTimeChanged;

        private void OnUpTimeChanged()
        {
            UpTimeChanged?.Invoke();
        }
    }
}
