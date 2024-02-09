using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

using Stride.Application.Common.Interfaces;
using Stride.Infrastructure.Data;
using Stride.Infrastructure.Data.Interceptors;
using Stride.Infrastructure.Identity;

namespace Stride.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        Guard.Against.Null(connectionString, message: $"Connection string was not found.");

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

        services.AddScoped<ApplicationDbContextInitialiser>();

        services
            .AddAuthentication(opt =>
            {
                opt.DefaultScheme = "BEARER_OR_COOKIE";
                opt.DefaultChallengeScheme = "BEARER_OR_COOKIE";
            })
            .AddBearerToken(IdentityConstants.BearerScheme)
            .AddCookie("Identity.Application", opt =>
            {
                opt.ExpireTimeSpan = TimeSpan.FromDays(7);
                opt.SlidingExpiration = true;
                opt.Cookie.Name = "Stride.Bearer";
                opt.Cookie.HttpOnly = true;
                opt.LoginPath = "/login";
            })
            .AddPolicyScheme("BEARER_OR_COOKIE", "BEARER_OR_COOKIE", options =>
            {
                options.ForwardDefaultSelector = context =>
                {
                    string? authorization = context.Request.Headers[HeaderNames.Authorization];
                    return !string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer ")
                        ? IdentityConstants.BearerScheme
                        : "Identity.Application";
                };
            });
        ;

        services.AddAuthorizationBuilder();

        services
            .AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddApiEndpoints();

        services.Configure<IdentityOptions>(opt =>
        {
            opt.Password.RequireDigit = false;
            opt.Password.RequireUppercase = false;
            opt.Password.RequireNonAlphanumeric = false;
        });

        services.AddSingleton(TimeProvider.System);
        services.AddTransient<IIdentityService, IdentityService>();

        return services;
    }
}
