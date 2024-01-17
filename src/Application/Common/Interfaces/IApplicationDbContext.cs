using Stride.Domain.Entities;

namespace Stride.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoItem> TodoItems { get; }
    DbSet<TodoList> TodoLists { get; }
    DbSet<StrideUser> StrideUsers { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}