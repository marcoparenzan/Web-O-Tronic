using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebOTronic.GameApp.Hubs;
using WebOTronic.GameApp.Services;
using Microsoft.Extensions.Hosting;
using WebOTronic.Core;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace WebOTronic.GameApp
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

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = nameof(GameHub),
                        ValidAudience = nameof(GameHub),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Authentication:SecurityKey"]))
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            if (!string.IsNullOrEmpty(accessToken))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(JwtBearerDefaults.AuthenticationScheme,
                policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireClaim(ClaimTypes.NameIdentifier);
                });
            });

            services.AddSingleton<IGameFabric, GameFabric>();

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // https://docs.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-2.1
            // https://stackoverflow.com/questions/44379560/how-to-enable-cors-in-asp-net-core-webapi
            services.AddCors(o => o.AddPolicy("Basic", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials()
                ;
            }));
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("Basic"));
            });

            services
                .AddSignalR()
                //.AddAzureSignalR(Configuration["AzureSignalR:ConnectionString"])
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

            app.UseCors("Basic");

            app.UseFileServer();

            app.UseSignalR(routes =>
            {
                routes.MapHub<GameHub>("/game");
            });

            //app.UseAzureSignalR(routes =>
            //{
            //    routes.MapHub<RemotePlay2Hub>("/game");
            //});
        }
    }
}
