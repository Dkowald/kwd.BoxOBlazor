using System;
using ForBrowser.Services;
using kwd.BoxOBlazor.Demo.Services.Clock;

namespace ForBrowser.Model
{
    public class AppState
    {
        public DateTime ServerTime;
        public TimeSpan UpTime;
        public event Action UpTimeChanged;

        public AppState(UITimer timedCallbacks, IClock sysClock)
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