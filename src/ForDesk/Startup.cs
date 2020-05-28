using kwd.BoxOBlazor.Demo;

using Microsoft.Extensions.DependencyInjection;

using WebWindows.Blazor;

namespace ForDesk
{
    class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddScoped<JsProxy>();
		}

		public void Configure(DesktopApplicationBuilder app)
		{
			app.AddComponent<App>("app");
		}
	}
}
