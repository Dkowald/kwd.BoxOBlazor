using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace kwd.BoxOBlazor.Services
{
    /// <summary>
    /// Interop calls for Browser LocalStorage object.
    /// </summary>
    public class LocalStorage : IDisposable
    {
        private readonly IJSRuntime _interop;
        private readonly ILogger<LocalStorage> _log;
        private DotNetObjectReference<LocalStorage> _jsRef;
        
        public LocalStorage(IJSRuntime interop, ILogger<LocalStorage> log)
        {
            _interop = interop;
            _log = log;
        }

        public async Task<bool> IsSupported()
            => await _interop.InvokeAsync<bool>("kwd.BoxOBlazor.Util.hasLocalStorage");

        public async Task Clear()
            => await _interop.InvokeVoidAsync("window.localStorage.clear");

        public async Task<string> Key(int index)
        {
            _log.LogTrace("Call local storage key");
            return await _interop.InvokeAsync<string>("window.localStorage.key", index);
        }

        public async Task<string> GetItem(string key)
        {
            _log.LogTrace("Get {key} from local storage", key);

            return await _interop.InvokeAsync<string>("window.localStorage.getItem", key);
        }

        public async Task<string> SetItem(string key, string value)
        {
            _log.LogTrace("Set {key} in local storage", key);
            
            return await _interop.InvokeAsync<string>("localStorage.setItem", key, value);
        }

        public async Task RemoveItem(string key)
            => await _interop.InvokeVoidAsync("localStorage.removeItem", key);
        
        public async Task<int> GetLength()
            => await _interop.InvokeAsync<int>("kwd.BoxOBlazor.Util.getLength");
        
        #region Forward document Storage event
        /// <summary>
        /// True if localStorage event is attached.
        /// </summary>
        public bool IsStorageEventAttached => _jsRef != null;

        /// <summary>
        /// Connect to browser storage event.
        /// Returns true if new link created, else false.
        /// </summary>
        public async Task<bool> HookStorageEvent()
        {
            _log.LogDebug("Attaching to document storage event");
            if (_jsRef != null)
            {
                _log.LogDebug("Already hooked to local storage event");
                return false;
            }

            _jsRef = DotNetObjectReference.Create(this);

            await _interop.InvokeVoidAsync("kwd.BoxOBlazor.Util.HookStorageEvent", _jsRef);

            _log.LogInformation("Attached to document storage event");

            return true;
        }
        
        /// <summary>
        /// Raised when browser local storage is updated.
        /// </summary>
        public event Action Storage;

        [JSInvokable]
        public void HandleStorageEvent()
        {
            _log.LogTrace("Raise local storage change event.");
            Storage?.Invoke();
        }
        #endregion

        #region IDispose
        public void Dispose()
        {
            _jsRef?.Dispose();
            _jsRef = null;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// https://developer.mozilla.org/en-US/docs/Web/API/StorageEvent
        /// </remarks>
        public class StorageEvent
        {
            public readonly string Key;
            public readonly string NewValue;
            public readonly string OldValue;

            public readonly string Url;
        }
    }
}