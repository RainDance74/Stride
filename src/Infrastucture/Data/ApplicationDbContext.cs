using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Stride.Application.Common.Interfaces;
using Stride.Domain.Entities;
using Stride.Infrastucture.Identity;
using System.Reflection;

namespace Stride.Infrastucture.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options), IApplicationDbContext
{
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    public DbSet<TodoList> TodoLists => Set<TodoList>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}
