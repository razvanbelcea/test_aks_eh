using eathappy.order.api.Extensions;
using eathappy.order.domain.Dtos.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace eathappy.order.api
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables();

            Configuration = configBuilder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<DynamicsPlatformConfig>(Configuration.GetSection(nameof(DynamicsPlatformConfig)));
            services.RegisterSwaggerForm();
            services.RegisterCors();
            services.AddControllersWithConfiguredSerialization();
            services.AddSwagger(Configuration);
            services.AddApplicationHealthChecks(Configuration);
            services.RegisterApplicationLayers(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.ConfigureExceptionHandlerMiddleware();
            app.ConfigureSwagger();
            //app.SeedData();
            app.ConfigureHealthChecks();
            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("CorsPolicy");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
