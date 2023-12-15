using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PBRmats.Core.Entities;
using PBRmats.Persistence.Data.Context;
using PBRmats.Repositories.Interfaces;
using PBRmats.Repositories.Repos;

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

            builder.Services.AddDbContext<PBRmatsContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<IRepository<AppUser, string>, Repository<AppUser, string>>();
            builder.Services.AddScoped<IRepository<License, int>, Repository<License, int>>();
            builder.Services.AddScoped<IRepository<Category, int>, Repository<Category, int>>();
            builder.Services.AddScoped<IRepository<Tag, int>, Repository<Tag, int>>();
            builder.Services.AddScoped<IRepository<Material, int>, Repository<Material, int>>();
            builder.Services.AddScoped<IRepository<MaterialsCollection, int>, Repository<MaterialsCollection, int>>();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<PBRmatsContext>();

            builder.Services.AddCors(option =>
            {
                option.AddPolicy("ApplicationCorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });

            var app = builder.Build();

            using var scope = app.Services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            CreateRoles(roleManager).Wait();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            AssignUserRoleToAllUsers(userManager, roleManager).Wait();

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

            app.UseCors("ApplicationCorsPolicy");
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
            if (!await roleManager.RoleExistsAsync("RootAdmin"))
            {
                await roleManager.CreateAsync(new IdentityRole("RootAdmin"));
            }

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }
        }

        private static async Task AssignUserRoleToAllUsers(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (await roleManager.RoleExistsAsync("User"))
            {
                var allUsers = await userManager.Users.ToListAsync();

                foreach (var user in allUsers)
                {
                    if (!await userManager.IsInRoleAsync(user, "User"))
                    {
                        await userManager.AddToRoleAsync(user, "User");
                    }
                }
            }
        }
    }
}