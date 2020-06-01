using System;

namespace kwd.BoxOBlazor.Demo.Services.Time
{
    /// <summary>Service to get time.</summary>
    public interface IClock
    {
        DateTime Now { get; }
    }
}