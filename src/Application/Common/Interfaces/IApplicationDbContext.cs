using Microsoft.EntityFrameworkCore;
using Stride.Domain.Entities;

namespace Stride.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoItem> TodoItems { get; }
    DbSet<TodoList> TodoLists { get; }
    DbSet<User> StrideUsers { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}