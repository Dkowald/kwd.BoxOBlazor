using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace kwd.BoxOBlazor.Server
{
    public static class Program
	{
		public static async Task<int> Main(string[] args)
		{
			await CreateHostBuilder(args).Build().RunAsync();

            return 0;
        }

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStaticWebAssets();
					webBuilder.UseStartup<Startup>();
				});
    }
}
