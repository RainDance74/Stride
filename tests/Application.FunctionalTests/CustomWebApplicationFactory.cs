using System.Data.Common;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Stride.Application.Common.Interfaces;
using Stride.Infrastructure.Data;

using static Stride.Application.FunctionalTests.Testing;

namespace Stride.Application.FunctionalTests;

public class CustomWebApplicationFactory(DbConnection connection)
    : WebApplicationFactory<Program>
{
    private readonly DbConnection _connection = connection;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services
                .RemoveAll<IUser>()
                .AddTransient(provider => Mock.Of<IUser>(s => s.Id == GetUserId()));

            services
                .RemoveAll<DbContextOptions<ApplicationDbContext>>()
                .AddDbContext<ApplicationDbContext>((sp, options) =>
                {
                    options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                    options
                    .UseNpgsql(_connection)
                    .UseSnakeCaseNamingConvention();
                });
        });
    }
}
