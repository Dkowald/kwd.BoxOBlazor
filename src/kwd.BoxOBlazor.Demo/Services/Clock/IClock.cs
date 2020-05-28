using System;

namespace kwd.BoxOBlazor.Demo.Services.Clock
{
    /// <summary>Service to get time.</summary>
    public interface IClock
    {
        DateTime Now { get; }
    }
}