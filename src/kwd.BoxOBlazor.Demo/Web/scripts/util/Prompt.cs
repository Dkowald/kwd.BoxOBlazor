using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace kwd.BoxOBlazor.Demo.Web.util
{
	public class Prompt
	{
		public static readonly string Namespace = 
			"kwd.BoxOBlazor.Util.";

		private readonly IJSRuntime _proxy;
		public Prompt(IJSRuntime proxy)
		{
			_proxy = proxy;
		}

		public async Task<string> ShowPrompt(string title, string prompt)
			=> await _proxy.InvokeAsync<string>(
				Namespace+"showPrompt", title, prompt);
	}
}