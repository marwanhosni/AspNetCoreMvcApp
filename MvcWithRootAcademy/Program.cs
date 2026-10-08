using Demo.DAL.Data;
using Demo.BLL.Repo.Abstract;
using Demo.BLL.Repo.Implementation;
using Microsoft.EntityFrameworkCore;
using Drmo.BLL.Interfaces;
using Drmo.BLL.Repositories;
using Demo.pl.Helper;
using Drmo.BLL;
using Microsoft.AspNetCore.Identity;
using Demo.DAL.Models;

namespace MvcWithRootAcademy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            //builder.Services.AddScoped<UserManager<ApplicationUser>>();
            //builder.Services.AddScoped<SignInManager<ApplicationUser>>();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                            .AddEntityFrameworkStores<ApplicationDbContext>()
                            .AddDefaultTokenProviders();
            builder.Services.ConfigureApplicationCookie(configure => {
                configure.LoginPath = "/Account/SignIn";
                configure.AccessDeniedPath = "/Account/AccessDenied";
                }); 

            builder.Services.AddAutoMapper(M => M.AddProfile(new MapProfile()));

            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change th is for production scenarios, see https://aka.ms/aspnetcore-hsts.
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

            app.Run();
        }
    }
}
