using SmartSupplyChain.API.Middleware;
using SmartSupplyChain.Infrastructure.Data.Seed;

namespace SmartSupplyChain.API.Extensions;

public static class ApplicationExtensions
{
    public static WebApplication UseApplicationExtensions(
        this WebApplication app)
    {

        app.UseCors("FrontendPolicy");

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseMiddleware<RequestLoggingMiddleware>();

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        using (var scope = app.Services.CreateScope())
        {
            var seeder = scope.ServiceProvider
                .GetRequiredService<DatabaseSeeder>();

            seeder.SeedAsync().GetAwaiter().GetResult();
        }

        return app;
    }
}