using MicroElements.Swashbuckle.FluentValidation.AspNetCore;

using Microsoft.OpenApi.Models;

using Stride.Application.Common.Interfaces;
using Stride.Infrastructure.Data;
using Stride.Web.Services;

namespace Stride.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddScoped<IUser, CurrentUser>();

        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        services.AddExceptionHandler<CustomExceptionHandler>();

        services.AddControllers();
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(configure =>
        {
            configure.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Stride API", Version = "v1" });

            // Add the fluent validations schema processor


            // Add JWT
            configure.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token.",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });

            configure.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        services.AddFluentValidationRulesToSwagger();

        return services;
    }
}
