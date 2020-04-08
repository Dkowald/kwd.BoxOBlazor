using System;

namespace ForServer.Services
{
    public interface IClock
    {
        public DateTime Now { get; }
    }

    public class DefaultClock : IClock
    {
        public DateTime Now => DateTime.UtcNow;
    }
}