using System;

namespace kwd.BoxOBlazor.Demo.Services.Time
{
    /// <summary>Simple <see cref="DateTime.Now"/> abstraction.</summary>
    public class DefaultClock : IClock
    {
        public DateTime Now => DateTime.UtcNow;
    }
}