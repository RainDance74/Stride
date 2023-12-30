using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Stride.Application.Common.Interfaces;
using Stride.Infrastructure.Data;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionStringEnvironmentKey = configuration.GetConnectionString("EnvironmentKey") 
            ?? throw new ArgumentNullException("A key for system variable of your connection string was not found in the appsettings.");
        var connectionString = Environment.GetEnvironmentVariable(connectionStringEnvironmentKey, EnvironmentVariableTarget.User);

        Guard.Against.Null(connectionString, message: $"Connection string was not found. " 
            + $"First, create a variable for \"{connectionStringEnvironmentKey}\" and put your connection string there.");

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            options
            .UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        return services;
    }
}
