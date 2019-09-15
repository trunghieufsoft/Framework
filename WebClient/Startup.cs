using System;
using Autofac;
using Serilog;
using System.IO;
using System.Text;
using Mapper.Config;
using Newtonsoft.Json;
using System.Reflection;
using Asset.Common.Timing;
using DataAccess.DbContext;
using Microsoft.AspNetCore.Http;
using BusinessAccess.Repository;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.Configuration;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WebClient.Configurations;

namespace WebClient
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            Log.Logger = new Serilog.LoggerConfiguration()
                .MinimumLevel.Information()
                //.WriteTo.RollingFile("Log-{Date}.txt", retainedFileCountLimit: 2)
                .CreateLogger();

            // Setting clock provider, using local time
            Clock.Provider = new UtcClockProvider();
            Log.Information("Web Start");
        }

        public IContainer ApplicationContainer { get; private set; }
        public AutofacServiceProvider provider;

        private void OnShutdown()
        {
            Log.Information("Shutdown");
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            #region Add migration for DbContext
            services.AddTransient<DbInitializer>();
            services.AddDbContext<DataAccess.DbContext.DataDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Connection")));
            #endregion

            #region Register AutoMapper
            var mapperFactory = MapperConfig.Register.AutoMapperConfig(services);
            services.AddSingleton<IMapperFactory>(mapperFactory);
            #endregion

            #region Add Configuration to dependency injection
            services.AddSingleton(Configuration);
            #endregion

            #region Add Repository
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            #endregion

            #region Add Swagger Documentation
            if (Configuration.GetValue<bool>("Config:EnableSwagger"))
            {
                services.AddSwaggerDocumentation();
            }
            #endregion

            #region Add Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.IncludeErrorDetails = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // The signing key must match!
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = Configuration["Jwt:Issuer"],

                    // Validate the token expiry
                    ValidateLifetime = false
                };
                options.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();

                        c.Response.StatusCode = 401;
                        c.Response.ContentType = "text/plain";

                        return c.Response.WriteAsync(c.Exception.ToString());
                    }
                };
            });

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //    consumerKey: "",
            //    consumerSecret: "");

            //app.UseFacebookAuthentication(
            //    appId: "426f62526f636b73",
            //    appSecret: "57686f6120447564652c2049495320526f636b73");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "477522346600.apps.googleusercontent.com",
            //    ClientSecret = "gobkdpbocikdfbnfahjladnetpdkvmic"
            //});
            #endregion

            #region Cookie Policy
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            #endregion

            #region Config Route
            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
            });

            services
                .AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.Formatting = Formatting.Indented;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });
            #endregion

            // Create the container builder.
            ContainerBuilder builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterModule<BusinessAccess.ModuleInit>();

            ApplicationContainer = builder.Build();
            provider = new AutofacServiceProvider(ApplicationContainer);
            return provider;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [Obsolete]
        public void Configure(IApplicationBuilder app, DbInitializer dbInitializer)
        {
            var applicationLifetime = app.ApplicationServices.GetRequiredService<IApplicationLifetime>();
            applicationLifetime.ApplicationStopping.Register(OnShutdown);

            #region Add Serilog
            ILoggerFactory loggerFactory = app.ApplicationServices.GetService<ILoggerFactory>();
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddSerilog();
            #endregion

            #region Environment
            IHostingEnvironment env = app.ApplicationServices.GetService<IHostingEnvironment>();
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
            #endregion

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            #region UseServiceMvc
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            #endregion

            #region Use Swagger Documentation
            //This line enables the app to use Swagger, with the configuration in the ConfigureServices method.
            app.UseSwagger();

            //This line enables Swagger UI, which provides us with a nice, simple UI with which we can view our API calls.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger XML Api v1");
            });
            #endregion

            app.UseAuthentication();

            // Seed Data
            dbInitializer.Seed().Wait();
        }
    }
}
