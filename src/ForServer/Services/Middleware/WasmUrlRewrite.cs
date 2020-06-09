using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using ForServer.Model;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ForServer.Services.Middleware
{
    public class WasmUrlRewrite : IMiddleware
    {
        private readonly string _wasmFiles;
        private readonly string _indexAsset;

        public static IServiceCollection Add(IServiceCollection services)
        { services.AddSingleton<WasmUrlRewrite>(); return services; }

        public static IApplicationBuilder Use(IApplicationBuilder app)
        { app.UseMiddleware<WasmUrlRewrite>(); return app;}

        public WasmUrlRewrite(IOptions<SiteConfig> config, IWebHostEnvironment env)
        {
            var siteConfig = config.Value;
            _wasmFiles = Path.Combine(
                Directory.GetCurrentDirectory(), siteConfig.WasmFileRoot);

            _indexAsset = Path.Combine(env.WebRootPath, "/wasm/index.asset.json");
        }

        public async Task InvokeAsync(HttpContext ctx, RequestDelegate next)
        {
            if (ctx.Request.Path.StartsWithSegments("/wasm"))
            {
                await SendIndexPage(ctx);
                return;
            }

            await next(ctx);
        }

        private async Task SendIndexPage(HttpContext ctx)
        {
            ctx.Response.StatusCode = (int)HttpStatusCode.OK;
            ctx.Response.ContentType = "text/html";

            var body = await File.ReadAllTextAsync(Path.Combine(_wasmFiles, "index.html"));
            //body = body.Replace("<base href=\"/\" />", "<base href=\"/wasm/\" />");
            await using var wr = new StreamWriter(ctx.Response.Body);
            await wr.WriteAsync(body);
        }

        private async Task CreateIndexPage()
        {
            var body = await File.ReadAllTextAsync(Path.Combine(_wasmFiles, "index.html"));
            body = body.Replace("<base href=\"/\" />", "<base href=\"/wasm/\" />");

            using var hasher = SHA256.Create();
            var assetHash = "sha256-" +
                        Convert.ToBase64String(hasher.ComputeHash(Encoding.UTF8.GetBytes(body)));

            await File.WriteAllLinesAsync(_indexAsset, new[]
            {
                "self.assetsCustom = {[",
                  "{",
                    $"\"hash\":\"{assetHash}\", \"url\": \"index.html\",",
                    "\"url\":\"index.html\"",
                   "}"+
                "]}"
            });
        }
    }
}