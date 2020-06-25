using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace kwd.BoxOBlazor.Demo.util
{
    public class Clipboard
    {
        private readonly IJSRuntime _proxy;
        private const string JSNamespace = "kwd.BoxOBlazor.Util.Clipboard";

        public Clipboard(IJSRuntime proxy)
        {
            _proxy = proxy;
        }

        /// <summary>
        /// Copy text content from given element to the clipboard.
        /// </summary>
        public async Task CopyFromElement(ElementReference elem)
            => await _proxy.InvokeVoidAsync(JSNamespace + ".copyFromElement", elem);

        /// <summary>
        /// Copy text to clipboard
        /// </summary>
        public async Task CopyToClipboard(string text)
            => await _proxy.InvokeVoidAsync(JSNamespace + ".copyToClipboard", text);
    }
}
