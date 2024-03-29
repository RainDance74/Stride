﻿using Stride.Application.Common.Exceptions;
using Stride.Application.Common.Interfaces;
using Stride.Domain.Entities;
using Stride.Domain.Events;

namespace Stride.Application.TodoItems.Commands.CreateTodoItem;

public record CreateTodoItemCommand : IRequest<int>
{
    public int ListId { get; init; }

    public string Title { get; init; } = null!;
}

public class CreateTodoItemCommandHandler(IApplicationDbContext context, IUser user) : IRequestHandler<CreateTodoItemCommand, int>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IUser _user = user;

    public async Task<int> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        TodoList? targetList = await _context.TodoLists
            .FirstOrDefaultAsync(l => l.Id == request.ListId, cancellationToken);

        Guard.Against.NotFound(request.ListId, targetList);

        Guard.Against.Null(_user.Id);

        if(targetList.Owner != _user.Id)
        {
            throw new ForbiddenAccessException("User should be owner of the target list.");
        }

        var entity = new TodoItem
        {
            TodoList = targetList,
            Title = request.Title,
            Done = false
        };

        entity.AddDomainEvent(new TodoItemCreatedEvent(entity));

        _context.TodoItems.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}

