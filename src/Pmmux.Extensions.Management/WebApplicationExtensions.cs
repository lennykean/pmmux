using System;
using System.Reflection;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;

namespace Pmmux.Extensions.Management;

internal static class WebApplicationExtensions
{
    public static WebApplication UseManagementUi(this WebApplication app, Uri apiBaseAddress)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var fileProvider = new ManifestEmbeddedFileProvider(assembly, "wwwroot");

        app.MapGet("/config.json", () =>
        {
            return Results.Json(new
            {
                BaseAddress = apiBaseAddress
            });
        });

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = fileProvider,
            ContentTypeProvider = new FileExtensionContentTypeProvider()
            {
                Mappings =
                {
                    [".dat"] = "application/octet-stream"
                }
            }
        });

        app.MapFallback(async context =>
        {
            var index = fileProvider.GetFileInfo("index.html");
            if (index.Exists)
            {
                context.Response.ContentType = "text/html";
                await using var stream = index.CreateReadStream();
                await stream.CopyToAsync(context.Response.Body).ConfigureAwait(false);
            }
            else
            {
                context.Response.StatusCode = 404;
            }
        });

        return app;
    }
}
