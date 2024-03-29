﻿using Stride.Application.Common.Exceptions;
using Stride.Application.Common.Interfaces;

namespace Stride.Application.TodoItems.Commands.UpdateTodoItem;

public record UpdateTodoItemCommand : IRequest
{
    public int Id { get; init; }

    public string Title { get; init; } = null!;

    public bool Done { get; init; }
}

public class UpdateTodoItemCommandHandler(IApplicationDbContext context, IUser user) : IRequestHandler<UpdateTodoItemCommand>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IUser _user = user;

    public async Task Handle(UpdateTodoItemCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.TodoItem? entity = await _context.TodoItems
            .Include(i => i.TodoList)
            .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        Guard.Against.Null(_user.Id);

        if(entity.TodoList.Owner != _user.Id)
        {
            throw new ForbiddenAccessException("User should be owner of the target list.");
        }

        entity.Title = request.Title;
        entity.Done = request.Done;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
