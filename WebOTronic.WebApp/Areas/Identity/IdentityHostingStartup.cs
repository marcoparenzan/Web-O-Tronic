using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebOTronic.WebApp.Areas.Identity.Data;

[assembly: HostingStartup(typeof(WebOTronic.WebApp.Areas.Identity.IdentityHostingStartup))]
namespace WebOTronic.WebApp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services
                    .AddIdentity<WebOTronicWebAppUser, ElCamino.AspNetCore.Identity.AzureTable.Model.IdentityRole>((options) =>
                    {
                        options.User.RequireUniqueEmail = true;
                    })
                    .AddAzureTableStoresV2<WebOTronicWebAppContext>(() =>
                    {
                        return new ElCamino.AspNetCore.Identity.AzureTable.Model.IdentityConfiguration
                        {
                            TablePrefix = context.Configuration["Storage:TablePrefix"],
                            StorageConnectionString = context.Configuration["Storage:ConnectionString"],
                            LocationMode = context.Configuration["Storage:LocationMode"]
                        };
                    }).CreateAzureTablesIfNotExists<WebOTronicWebAppContext>().AddDefaultTokenProviders();

                services.ConfigureApplicationCookie(options => options.LoginPath = "/Identity/Account/Login");
                services.AddAuthentication().AddFacebook(fb =>
                {
                    fb.AppId = context.Configuration["Facebook:AppId"];
                    fb.AppSecret = context.Configuration["Facebook:AppSecret"];
                    fb.CallbackPath = context.Configuration["Facebook:CallbackPath"]; ;
                });

            });
        }
    }
}