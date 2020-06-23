using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using BlazorStamp.Model;

using Microsoft.AspNetCore.Blazor.Build;
using Microsoft.Extensions.Configuration;

namespace BlazorStamp
{
    /// <summary>
    /// Updates a published Blazor WASM site.
    /// </summary>
    public class Stamper
    {
        //text prefixed to asset file by blazor build (GenerateServiceWorkerAssetsManifest)
        private const string AssetFilePrefix = "self.assetsManifest = ";

        private const string BaseElementPattern = "<base\\s*href\\s*=\\s*\".*\"\\s*/>";

        private readonly StamperSettings _config;

        private readonly FileInfo _indexHtml;
        private readonly FileInfo _assetsManifest;

        public Stamper(IConfiguration config)
            :this(config.Get<StamperSettings>()){}

        public Stamper(StamperSettings site)
        {
            _config = site;

            _indexHtml = new FileInfo(
                Path.Combine(site.TargetPath, site.DefaultFile));

            _assetsManifest = new FileInfo(
                Path.Combine(site.TargetPath, "service-worker-assets.js"));
        }

        /// <summary>
        /// Updates the site file(s)
        /// </summary>
        public async Task<int> Execute()
        {
            Console.WriteLine("Updating files in '"+
                              Path.GetFullPath(_config.TargetPath) + "'");
            
            //replace index html.
            if (_indexHtml.Exists && !string.IsNullOrWhiteSpace(_config.BaseUrl))
            {
                Console.WriteLine("Updating base element in default file");
                await UpdateBaseElement();
            }

            //re-do assets data.
            if (_assetsManifest.Exists)
            {
                Console.WriteLine("Updating asset");
                await RebuildAssetFile();
                await GzCompressAssetFile();
                await BrCompressAssetFile();
            }

            if (!_indexHtml.Exists && !_assetsManifest.Exists)
            {return -1;}

            return 0;
        }

        private async Task UpdateBaseElement()
        {
            var body = await File.ReadAllTextAsync(_indexHtml.FullName);

            var baseElement = $"<base href=\"{_config.BaseUrl}\" />";
            
            body = Regex.Replace(body, BaseElementPattern, baseElement);
            
            await File.WriteAllTextAsync(_indexHtml.FullName, body);
        }

        private async Task RebuildAssetFile()
        {
            var body = await File.ReadAllTextAsync(_assetsManifest.FullName);
            body = body.Substring(AssetFilePrefix.Length).Trim()
                .TrimEnd(';');

            var manifest = JsonSerializer.Deserialize<GenerateServiceWorkerAssetsManifest.AssetsManifestFile>(body);

            using var hasher = SHA256.Create();

            foreach (var asset in manifest.assets)
            {
                var path = Path.Combine(_config.TargetPath, asset.url);
                await using var rd = File.OpenRead(path);

                var h = hasher.ComputeHash(rd);

                asset.hash = $"sha256-{Convert.ToBase64String(h)}";
            }

            await using var wr = File.Create(_assetsManifest.FullName);

            await using var txt = new StreamWriter(wr);
            await txt.WriteLineAsync(AssetFilePrefix);
            await txt.FlushAsync();

            await JsonSerializer.SerializeAsync(wr, manifest, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            await txt.WriteLineAsync(";");
        }

        private async Task GzCompressAssetFile()
        {
            var gz = _assetsManifest.FullName + ".gz";
            await using var sourceStream = File.OpenRead(_assetsManifest.FullName);
            await using var fileStream = new FileStream(gz, FileMode.Create);
            await using var stream = new GZipStream(fileStream, CompressionLevel.Optimal);
            await sourceStream.CopyToAsync(stream);
        }

        private async Task BrCompressAssetFile()
        {
            var br = _assetsManifest.FullName + ".br";

            await using var sourceStream = File.OpenRead(_assetsManifest.FullName);
            await using var fileStream = new FileStream(br, FileMode.Create);
            await using var stream = new BrotliStream(fileStream, CompressionLevel.Optimal);
            await sourceStream.CopyToAsync(stream);
        }
    }
}