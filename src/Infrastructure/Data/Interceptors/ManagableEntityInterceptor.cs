using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

using Stride.Application.Common.Interfaces;
using Stride.Domain.Common;

namespace Stride.Infrastructure.Data.Interceptors;

public class ManagableEntityInterceptor(
    IUser user,
    IServiceProvider serviceProvider) : SaveChangesInterceptor
{
    private readonly IUser _user = user;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if(context == null)
        {
            return;
        }

        foreach(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<BaseManageableEntity> entry in context.ChangeTracker.Entries<BaseManageableEntity>())
        {
            // TODO: Am I need this?
            Identity.ApplicationUser? user = _serviceProvider.GetRequiredService<ApplicationDbContext>().Users
                    .FirstOrDefault(u => u.Id == _user.Id);

            if(entry.State == EntityState.Added)
            {
                Guard.Against.Null(user);

                entry.Entity.CreatedBy = user.Id;
            }
            else if(entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                entry.Entity.UpdatedBy = user?.Id;
            }
        }
    }
}