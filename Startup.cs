using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Bicks.Models;
using Bicks.Services;
using Bicks.Data;
using Bicks.Data.DAL;
using Bicks.Library.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rotativa.AspNetCore;

namespace Bicks
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseLazyLoadingProxies()
                    .UseSqlServer(
                        Configuration.GetConnectionString("DatabaseConnection")));
            services.AddWorkUnits();

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Lockout.AllowedForNewUsers = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.LogoutPath = "/Identity/Account/Logout";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });

            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("DatabaseConnection")));
            services.AddHangfireServer();

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<IFileTemplateService, FileTemplateService>();



            services.AddScoped<IDbInitializer, DbInitializer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext dbContext, IDbInitializer dbInitializer)
        {
            dbContext.Database.Migrate();
            dbInitializer.Initialize();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new MyAuthorizationFilter() }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "Invoicing",
                    areaName: "Invoicing",
                    pattern: "Invoicing/{action=Index}/{id?}",
                    defaults: new { controller = "Invoicing", Area = "Invoicing" });
                endpoints.MapAreaControllerRoute(
                    name: "ProductManagement",
                    areaName: "ProductManagement",
                    pattern: "ProductManagement/{action=Index}/{id?}",
                    defaults: new { controller = "ProductManagement", Area = "ProductManagement" });
                endpoints.MapAreaControllerRoute(
                    name: "Sales",
                    areaName: "Sales",
                    pattern: "Sales/{action=Index}/{id?}",
                    defaults: new { controller = "Sales", Area = "Sales" });
                endpoints.MapAreaControllerRoute(
                    name: "ClientManagement",
                    areaName: "ClientManagement",
                    pattern: "ClientManagement/{action=Index}/{id?}",
                    defaults: new { controller = "ClientManagement", Area = "ClientManagement" });
                endpoints.MapAreaControllerRoute(
                    name: "Settings",
                    areaName: "Settings",
                    pattern: "Settings/{action=Index}/{id?}",
                    defaults: new { controller = "Settings", area = "Settings" });
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            RotativaConfiguration.Setup(env.WebRootPath, "Rotativa");
        }
    }
}
