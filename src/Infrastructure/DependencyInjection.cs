using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Stride.Application.Common.Interfaces;
using Stride.Infrastructure.Data;
using Stride.Infrastructure.Data.Interceptors;
using Stride.Infrastructure.Identity;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionStringEnvironmentKey = configuration
            .GetSection("EnvironmentKeys")
            .GetValue<string>("ConnectionString");

        Guard.Against.Null(connectionStringEnvironmentKey, message: "A key for system variable of your connection string was not found in the appsettings.");

        var connectionString = Environment.GetEnvironmentVariable(connectionStringEnvironmentKey, EnvironmentVariableTarget.User);

        Guard.Against.Null(connectionString, message: $"Connection string was not found. " 
            + $"First, create a variable for \"{connectionStringEnvironmentKey}\" and put your connection string there.");

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, ManagableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            options
            .UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddAuthentication()
            .AddBearerToken(IdentityConstants.BearerScheme);

        services.AddAuthorizationBuilder();

        services
            .AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddApiEndpoints();

        services.AddSingleton(TimeProvider.System);
        services.AddTransient<IIdentityService, IdentityService>();

        return services;
    }
}
