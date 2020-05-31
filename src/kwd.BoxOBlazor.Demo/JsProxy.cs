using kwd.BoxOBlazor.Demo.util;
using Microsoft.JSInterop;

namespace kwd.BoxOBlazor.Demo
{
	/// <summary>
	/// A top-level js proxy 
	/// </summary>
	/// <remarks>
	/// Keeping the c# code for <see cref="Prompt"/>
	/// close to the source js file.
	/// And use this as a top-level entrance.
	/// </remarks>
	public class JsProxy
	{
		private readonly IJSRuntime _jsRuntime;

		public JsProxy(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime;
		}

		public Prompt Util => new Prompt(_jsRuntime);
	}
}