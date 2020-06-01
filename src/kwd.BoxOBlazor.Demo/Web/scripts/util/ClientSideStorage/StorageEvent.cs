using System.Text.Json;

namespace kwd.BoxOBlazor.Demo.util.ClientSideStorage
{
    /// <summary>
    /// Present a browser storage event in .NET 
    /// </summary>
    /// <remarks>
    /// Based on: 
    /// https://developer.mozilla.org/en-US/docs/Web/API/StorageEvent
    /// </remarks>
    public class StorageEvent
    {
        /// <summary>
        /// Create payload based on json data from browser.
        /// </summary>
        public StorageEvent(JsonElement fromJson)
        {
            SelfTriggered = false;

            if (fromJson.TryGetProperty(nameof(Key), out var elem))
                Key = elem.GetString();

            if (fromJson.TryGetProperty(nameof(NewValue), out elem))
                NewValue = elem.GetString();

            if (fromJson.TryGetProperty(nameof(OldValue), out elem))
                OldValue = elem.GetString();

            if (fromJson.TryGetProperty(nameof(Url), out elem))
                Url = elem.GetString();
        }

        /// <summary>
        /// Create self-triggered event data.
        /// </summary>
        public StorageEvent(string key, string oldValue, string newValue, string url = null, bool selfTriggered = true)
        {
            Key = key;
            OldValue = oldValue;
            NewValue = newValue;
            Url = url;
            SelfTriggered = selfTriggered;
        }

        /// <summary>True if change triggered from this circuit (browser tab)</summary>
        public readonly bool SelfTriggered;

        /// <summary>The Key that changed (null if all keys cleared)</summary>
        public readonly string Key;

        /// <summary>The new Key value (null if key removed)</summary>
        public readonly string NewValue;

        /// <summary>The old key value (null if new item)</summary>
        public readonly string OldValue;

        /// <summary>Document url</summary>
        public readonly string Url;
    }
}