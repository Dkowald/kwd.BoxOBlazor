using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using BlazorStamp.Model;

using Microsoft.Extensions.Configuration;

namespace BlazorStamp
{
    static class Program
    {
        static async Task<int> Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            var settings = config.Get<StamperSettings>();

            settings.TargetPath ??= Directory.GetCurrentDirectory();

            if (args.Any(x => x == "?" ||
                    string.Equals(x, "help", StringComparison.OrdinalIgnoreCase)))
            { Help(); return 1; }

            var engine = new Stamper(settings);

            var result = await engine.Execute();

            if(result != 0){Help();}

            return result;
        }

        private static void Help()
        {
            var msg =
                "A cli tool to update blazor wasm assets.\n" +
                "----------------------------------------\n" +
                "Actions\n"+
                "help (or ?) to see this\n"+
                "----------------------------------------\n" +
                "Options\n" +
                "--TargetPath=<path to wasm files> (default to cwd)\n" +
                "--BaseUrl=<base-element-url> (optional)\n" +
                "--DefaultFile=<index.html>\n " +
                "----------------------------------------\n";
                
            Console.Write(msg);
        }
    }
}
