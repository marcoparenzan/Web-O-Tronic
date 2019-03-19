using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebOTronic.WebApp.Models;
using WebOTronic.WebApp.Hubs;
using WebOTronic.WebApp.Services;
using Microsoft.Extensions.Hosting;
using WebOTronic.WebApp.Areas.Identity.Data;

namespace WebOTronic.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //This method gets called by the runtime.Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSingleton<IRemotePlay2Service, RemotePlay2Service>();

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services
                .AddSignalR()
                .AddAzureSignalR(Configuration["AzureSignalR:ConnectionString"])
            ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseFileServer();

            //app.UseSignalR(routes =>
            //{
            //    routes.MapHub<RemotePlayHub>("/remoteplay");
            //    routes.MapHub<RemotePlay2Hub>("/remoteplay2");
            //});

            app.UseAzureSignalR(routes =>
            {
                routes.MapHub<RemotePlayHub>("/remoteplay");
                routes.MapHub<RemotePlay2Hub>("/remoteplay2");
            });
        }
    }
}
