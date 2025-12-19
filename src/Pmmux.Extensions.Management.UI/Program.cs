using System;
using System.Net.Http;
using System.Net.Http.Json;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

using Pmmux.Extensions.Management.UI;
using Pmmux.Extensions.Management.UI.Services;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
var httpClient = new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
};
var result = await httpClient.GetAsync("config.json");
var config = result.IsSuccessStatusCode && await result.Content.ReadFromJsonAsync<ManagementConfig>() is { } c
    ? c
    : new(builder.HostEnvironment.BaseAddress);

Console.WriteLine($"Management API base address: {config.BaseAddress}");
Console.WriteLine($"HostEnvironment.BaseAddress base address: {builder.HostEnvironment.BaseAddress}");
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(config.BaseAddress == "/" ? builder.HostEnvironment.BaseAddress : config.BaseAddress)
});
builder.Services.AddScoped<PmmuxApiClient>();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

await builder.Build().RunAsync();

record ManagementConfig(string BaseAddress);
