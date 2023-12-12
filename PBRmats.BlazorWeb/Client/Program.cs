using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;
using PBRmats.BlazorWeb.Services;
using PBRmats.Core.Entities;
using PBRmats.Persistence.Data.Context;
using PBRmats.Repositories.Interfaces;
using PBRmats.Repositories.Repos;

namespace PBRmats.BlazorWeb
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddScoped<LicenseService>();
            builder.Services.AddScoped<CategoryService>();
            builder.Services.AddScoped<TagService>();

            await builder.Build().RunAsync();
        }
    }
}