using Microsoft.EntityFrameworkCore;
using WebApi.Data;

namespace WebApi.Extensions;

public static class DbConnectionExtensions
{
    public static void AddDbContextConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
        });
    }
}
