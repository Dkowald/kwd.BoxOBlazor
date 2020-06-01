using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace kwd.BoxOBlazor.Demo.util
{
	public class Prompt
	{
        public const string JSNamespace = "kwd.BoxOBlazor.Util";

        private readonly IJSRuntime _proxy;
		public Prompt(IJSRuntime proxy)
		{
			_proxy = proxy;
		}

		public async Task<string> ShowPrompt(string title, string prompt)
			=> await _proxy.InvokeAsync<string>(
				JSNamespace+".showPrompt", title, prompt);
	}
}