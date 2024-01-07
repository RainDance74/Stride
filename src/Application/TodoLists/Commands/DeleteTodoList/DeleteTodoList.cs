﻿using Stride.Application.Common.Interfaces;

namespace Stride.Application.TodoLists.Commands.DeleteTodoList;

public record DeleteTodoListCommand(int Id) : IRequest;

public class DeleteTodoListCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteTodoListCommand>
{
    private readonly IApplicationDbContext _context = context;

    public async Task Handle(DeleteTodoListCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoLists
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        _context.TodoLists.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
