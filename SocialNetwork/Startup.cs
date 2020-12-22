using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.IO;
using SocialNetwork.Domain.Core;
using SocialNetwork.Domain.Interfaces;
using SocialNetwork.Infrastructure.Data;
using SocialNetwork.Services.BusinessLogic;

namespace SocialNetwork
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
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddControllersWithViews();
            services.AddSignalR(hubOptions =>
            {
                hubOptions.EnableDetailedErrors = true;
                hubOptions.KeepAliveInterval = TimeSpan.FromMinutes(1);
            });

            services.AddControllersWithViews(mvcOtions =>
            {
                mvcOtions.EnableEndpointRouting = false;
            });

            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddTransient<IMailService, MailService>();


            services.AddIdentity<User, IdentityRole>(opts =>
            {
                opts.Password.RequiredLength = 6;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<UsersContext>();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("ru")
                };

                options.DefaultRequestCulture = new RequestCulture("ru");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
            services.AddMvc()
             .AddDataAnnotationsLocalization()
             .AddViewLocalization();
           
            services.AddDbContext<UsersContext>(options =>
                  options.UseSqlServer(
                      Configuration.GetConnectionString("DefaultConnection"),
                      b => b.MigrationsAssembly("SocialNetwork")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [Obsolete]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            var path = Directory.GetCurrentDirectory();
            loggerFactory.AddFile($"{path}/Logs/mylog-All.txt", minimumLevel: LogLevel.Trace);
            loggerFactory.AddFile($"{path}/Logs/mylog-Error.txt", minimumLevel: LogLevel.Information);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePages();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            IApplicationBuilder applicationBuilder = app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "friends",
                    template: "friends",
                    defaults: new { controller = "User", action = "Friends" });

                routes.MapRoute(
                    name: "news",
                    template: "news",
                    defaults: new { controller = "News", action = "Index" });

                routes.MapRoute(
                    name: "messages",
                    template: "messages",
                    defaults: new { controller = "Message", action = "Index" });

                routes.MapRoute(
                    name: "search",
                    template: "search",
                    defaults: new { controller = "Search", action = "Index" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<ChatHub>("/chat");
  
            });
        }
    }
}
