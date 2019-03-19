 using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WebOTronic.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddEnvironmentVariables()
                .Build();

            var httpsPort = int.Parse(config["Https:Port"]);

            var host =
                (new WebHostBuilder())
                //.UseUrls($"https://*:{httpsPort}")
                .UseKestrel(options => {
                    options.Listen(IPAddress.Any, httpsPort, listenOptions =>
                    {
                        var deviceCertificateBytes = File.ReadAllBytes(config["Https:CertFileName"]);
                        var deviceCertificatePassword = config["Https:CertPassword"];
                        var deviceCertificate = (X509Certificate2)new X509Certificate2(deviceCertificateBytes, deviceCertificatePassword);
                        listenOptions.UseHttps(deviceCertificate);
                    });
                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseConfiguration(config)
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging
                        .AddConsole()
                        .AddDebug();
                })
                //.UseIISIntegration()
                //.UseDefaultServiceProvider((context, options) =>
                //{
                //    options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
                //})
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
