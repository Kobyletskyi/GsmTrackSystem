#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.StaticFiles;
using Repositories.DataBase;
using Repositories.UnitOfWork;
using BusinessLayer;
using Api.Providers.Auth;
using BusinessLayer.DB;
using BusinessLayer.Mappers;
using BusinessLayer.Transformation;
using Api.Extentions;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Swashbuckle;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Buffering;
#endregion

namespace Api
{
    public class Startup
    {
        const string Cookies = CookieAuthenticationDefaults.AuthenticationScheme;
        const string Bearer = JwtBearerDefaults.AuthenticationScheme;
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            string connectionString = Configuration.GetSection("ConnectionStrings").GetValue<string>("DbConnection");
            //services.AddDbContext<DbScheme>(options => options.UseInMemoryDatabase());//UseSqlServer(connection));            
            services.AddDbContext<DbScheme>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IUnitOfWorkRepositories, Repositories.Repositories>();
            services.AddScoped<IBusinessLogic, BusinessLogic>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddTransient<ITypeHelperService, TypeHelperService>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper, UrlHelper>(implementationFactory => {
                var actionContext = implementationFactory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });
            services.AddTransient<IPropertyMappingService, PropertyMappingService>();

            //services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policy.Admin, policy => policy.RequireClaim(AuthService.UserIdClaim));
                options.AddPolicy(Policy.User, policy => policy.RequireClaim(AuthService.UserIdClaim));
                options.AddPolicy(Policy.Device, policy => policy.RequireClaim(AuthService.ImeiClaim));
                // options.AddPolicy("User", policyBuilder =>
                // {
                //     policyBuilder.
                //     policyBuilder.RequireAuthenticatedUser()
                //         .RequireAssertion(context => context.User.HasClaim("Read", "true"))
                //         .Build();
                // });
            });

            
            services.AddAuthentication(o => {
                o.DefaultScheme = Bearer;
                o.DefaultAuthenticateScheme = Bearer;
                o.DefaultSignInScheme = Cookies;
                o.DefaultChallengeScheme = Cookies;
            })
            .AddJwtBearer(Bearer, options => {
                options.RequireHttpsMetadata = false;
                options.Audience = AuthConfig.Audience;
                options.Configuration = new OpenIdConnectConfiguration();
                options.Authority = AuthConfig.Authority;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = AuthConfig.ValidationParameters;
            })
            .AddCookie(Cookies, options => {
                options.LoginPath = new PathString("/login");
            });

            services.AddAntiforgery(
                opts =>
                {
                    opts.Cookie.Name = "_af";
                    opts.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                }
            );

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
                c.TagActionsBy(api => api.GroupName);
            });

            services.AddMvc(setupAction => {
                setupAction.ReturnHttpNotAcceptable = true;
            }).AddJsonOptions(options => {
                options.SerializerSettings.ContractResolver = 
                    new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddRouting(options => 
                options.ConstraintMap.Add("me", typeof(MeRouteConstraint)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment hostingEnvironment,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            DbScheme dbContext)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if(env.IsDevelopment()){
                app.UseDeveloperExceptionPage();
            }else{
                // app.UseExceptionHandler(appBuilder => {
                //     appBuilder.Run(async context =>{
                //         context.Response.StatusCode = 500;
                //         await context.Response.WriteAsync("error");
                //     });
                // });
            }

            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404
                    && !context.Request.Path.StartsWithSegments(new PathString("/api")))
                {
                    context.Request.Path = "/";
                    context.Response.StatusCode = 200;
                    context.Response.Headers.Remove("Content-Length");
                    await next();
                    context.Response.Headers.Remove("Content-Length");
                }
                
            });
            //ToDo: Tracker device does not support chunked responses
            app.UseResponseBuffering();

            app.UseAuthentication();

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(hostingEnvironment.WebRootPath)
            });
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            AutoMapper.Mapper.Initialize(cfg => {
                AutoMapperApiInitializer.Init(cfg);
                AutoMapperInitializer.Init(cfg);
            });
            
            
            new DbInitializer(dbContext).Seed("firstUser", new PasswordHash("password").ToString()).Wait();
        }
    }
}
