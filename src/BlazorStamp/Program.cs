using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace BlazorStamp
{
    static class Program
    {
        static async Task<int> Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
            
            var engine = new Stamper(config);

            await engine.Execute();

            return 0;
        }
    }
}
