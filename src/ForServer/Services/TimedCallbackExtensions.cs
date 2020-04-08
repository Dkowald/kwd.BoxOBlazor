using System;
using System.Threading;

namespace ForServer.Services
{
    /// <summary>
    /// Extensions for syntax sugar on <see cref="TimedCallback"/>.
    /// </summary>
    public static class TimedCallbackExtensions
    {
        /// <summary>
        /// Register a one time call back.
        /// </summary>
        public static string OneShot(this TimedCallback self,
            Action op, TimeSpan startDelay, string name = null)
            => self.Register(op, startDelay, Timeout.InfiniteTimeSpan, name);

        /// <summary>
        /// Register a repeating call-back.
        /// The call-back is NOT immediately called, it will be called after
        /// <paramref name="repeatPeriod"/>.
        /// </summary>
        public static string Repeater(this TimedCallback self,
            Action op, TimeSpan repeatPeriod, string name = null)
            => self.Register(op, repeatPeriod, repeatPeriod, name);
    }
}