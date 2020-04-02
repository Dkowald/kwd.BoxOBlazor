using System.Threading.Tasks;

using kwd.BoxOBlazor;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace ForBrowser
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("app");
			
			ConfigureServices(builder.Services);
			
			await builder.Build().RunAsync();
		}

		static void ConfigureServices(IServiceCollection services)
		{
			services.AddBaseAddressHttpClient();
			services.AddScoped<JsProxy>();
		}
	}
}
