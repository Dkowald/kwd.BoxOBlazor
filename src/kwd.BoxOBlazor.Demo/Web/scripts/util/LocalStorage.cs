using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace kwd.BoxOBlazor.Demo.Web.util
{
    /// <summary>
    /// Interop calls for Browser LocalStorage object.
    /// </summary>
    /// <remarks>
    /// Unlike the DOM;
    /// this raises self-triggered events corresponding to
    /// mutate methods.
    /// </remarks>
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
        {
            await _interop.InvokeVoidAsync("window.localStorage.clear");

            StorageUpdated?.Invoke(new StorageEvent(null, null, null, null));
        }

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

        public async Task SetItem(string key, string value)
        {
            _log.LogTrace("Set {key} in local storage", key);

            try
            {
                await _interop.InvokeVoidAsync("kwd.BoxOBlazor.Util.LocalStorage.setItem", key, value);

                StorageUpdated?.Invoke(new StorageEvent(
                    key, null, value, null));

            }
            catch (JSException jsError)
            {
                var error = EnrichError.TryUnwrap(jsError);

                _log.LogError("JSError: "+ error.Message);

                throw error;
            }
        }

        public async Task RemoveItem(string key)
        {
            await _interop.InvokeVoidAsync("localStorage.removeItem", key);

            StorageUpdated?.Invoke(new StorageEvent(key, null, null));
        }

        /// <summary>
        /// Number of items currently in local storage.
        /// </summary>
        public async Task<int> GetLength()
            => await _interop.InvokeAsync<int>("kwd.BoxOBlazor.Util.LocalStorage.getLength");
        
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
            _log.LogTrace("Attaching to document storage event");
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
        public event Action<StorageEvent> StorageUpdated;

        [JSInvokable]
        public void HandleStorageEvent(JsonElement oEvent)
        {
            var evt = new StorageEvent(oEvent);
            
            _log.LogTrace("Raise local storage change event.");

            StorageUpdated?.Invoke(evt);
        }
        #endregion

        #region IDispose
        public void Dispose()
        {
            _jsRef?.Dispose();
            _jsRef = null;
        }
        #endregion
    }
}