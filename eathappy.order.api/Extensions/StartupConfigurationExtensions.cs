using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace eathappy.order.api.Extensions
{
#pragma warning disable CS1591 // Unrecognized #pragma directive
    public static class StartupConfigurationExtensions
    {
        public static void ConfigureHealthChecks(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/.well-known/live", new HealthCheckOptions
            {
                Predicate = check => check.Tags.Contains("live")
            });

            app.UseHealthChecks("/.well-known/ready", new HealthCheckOptions
            {
                Predicate = check => check.Tags.Contains("ready")
            });
        }

        public static void ConfigureSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "EatHappy Orders API v1");
            });
        }

        public static void ConfigureExceptionHandlerMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware(typeof(ExceptionMiddleware));
        }

        //public static void SeedData(this IApplicationBuilder app)
        //{
        //    using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
        //    var dbContextOptions = scope.ServiceProvider.GetService<DbContextOptions<DocumentServiceContext>>();
        //    using var dbContext = new DocumentServiceContext(dbContextOptions);
        //    dbContext.Database.Migrate();
        //}
    }
#pragma warning restore CS1591 // Unrecognized #pragma directive
}
