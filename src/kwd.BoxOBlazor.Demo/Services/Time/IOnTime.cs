using System;
using System.Threading.Tasks;

namespace kwd.BoxOBlazor.Demo.Services.Time
{
    /// <summary>
    /// Service to register and execute an action
    /// some-time in the future.
    /// </summary>
    /// <remarks>
    /// It is incredibly useful to have a simple
    /// call back for some time in the future.
    /// </remarks>
    public interface IUITimers
    {
        /// <summary>
        /// Register named time event to call <paramref name="op"/>
        /// in the future.
        /// </summary>
        /// <param name="op">The method to be called; may execute on another thread</param>
        /// <param name="startDelay">Time to delay before calling the op</param>
        /// <param name="repeatPeriod">Optional repeating call to <paramref name="op"/></param>
        /// <param name="name">Optional name for this timer</param>
        public ValueTask<string> Register(Action op,
            TimeSpan startDelay,
            TimeSpan repeatPeriod,
            string name = null);

        /// <summary>
        /// Un-register a previously created timer.
        /// Safe to call multiple times.
        /// </summary>
        /// <param name="name">NAme for the timer.</param>
        /// <returns>True if the time was found and removed, else false.</returns>
        public ValueTask<bool> UnRegister(string name);
    }
}