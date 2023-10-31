using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PBRmats.Core.Context;
using PBRmats.Core.Entities;
using PBRmats.Repositories.Interfaces;
using PBRmats.Repositories.Repos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using System;
using System.Threading.Tasks;


namespace PBRmatsWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("PBRmatsConnection") ?? 
                throw new InvalidOperationException("Connection string 'PBRmatsConnection' not found.");

            builder.Services.AddDbContext<PBRmatsContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<IRepository<License, int>, Repository<License, int>>();
            builder.Services.AddScoped<IListService<License>, LicenseService>();
            builder.Services.AddScoped<IRepository<Category, int>, Repository<Category, int>>();
            builder.Services.AddScoped<IListService<Category>, CategoryService>();
            builder.Services.AddScoped<IRepository<Tag, int>, Repository<Tag, int>>();
            builder.Services.AddScoped<IRepository<Material, int>, Repository<Material, int>>();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<PBRmatsContext>();

            var app = builder.Build();

            using var scope = app.Services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            CreateRoles(roleManager).Wait();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
        private static async Task CreateRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }
        }
    }
}