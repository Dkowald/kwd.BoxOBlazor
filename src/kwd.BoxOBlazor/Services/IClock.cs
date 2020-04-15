using System;

namespace kwd.BoxOBlazor.Services
{
    public interface IClock
    {
        DateTime Now { get; }
    }

    public class DefaultClock : IClock
    {
        public DateTime Now => DateTime.UtcNow;
    }
}