using FluentValidation;
using hu_app.Hubs;
using hu_app.Shared;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Reflection;

namespace hu_app
{
    public class Startup
    {
        public IConfiguration _config { get; }

        public Startup(IConfiguration config)
        {
            _config = config;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureMediatR(services);
            ConfigureDataAccess(services);
            ConfigureApplication(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            app.UseHuExceptionHandler();
            //app.UseDeveloperExceptionPage();

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Lax
            });

            app.UseStaticFiles();

            //app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
                endpoints.MapHub<DashboardHub>("/DashboardHub");
            });
            IdentityModelEventSource.ShowPII = true;
        }

        private void ConfigureMediatR(IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(HuMediatorPerformance<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(HuMediatorValidation<,>));
        }

        private void ConfigureDataAccess(IServiceCollection services)
        {
            var connectionString = _config.GetConnectionString("HuDb");
            HuLogger.Setup(connectionString);
            services.AddDbContext<HuDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
                //options.EnableSensitiveDataLogging(true);
            });
            services.AddScoped(typeof(HuRepository<>));
        }

        private void ConfigureApplication(IServiceCollection services)
        {
            services.AddSession();
            services.AddHttpContextAccessor();
            services.AddSignalR();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddRazorPages()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AddPageRoute("/Dashboard", "/");
                })
                .AddRazorRuntimeCompilation();

            services.AddScoped<HuServicesBag>();
            //services.AddHostedService<HuWorker>();
        }
    }
}
