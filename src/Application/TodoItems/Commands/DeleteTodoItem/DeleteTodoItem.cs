using Stride.Application.Common.Interfaces;
using Stride.Domain.Events;

namespace Stride.Application.TodoItems.Commands.DeleteTodoItem;

public record DeleteTodoItemCommand(int Id) : IRequest;

public class DeleteTodoItemCommandHandler(IApplicationDbContext context, IUser user) : IRequestHandler<DeleteTodoItemCommand>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IUser _user = user;

    public async Task Handle(DeleteTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoItems
            .Include(i => i.TodoList)
            .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        Guard.Against.Null(_user.Id);

        var currentUser = await _context.StrideUsers
            .FindAsync(new object[] { _user.Id }, cancellationToken);

        Guard.Against.NotFound(_user.Id, currentUser);

        if(entity.TodoList.Owner != currentUser)
        {
            throw new UnauthorizedAccessException("User should be owner of the target list.");
        }

        _context.TodoItems.Remove(entity);

        entity.AddDomainEvent(new TodoItemDeletedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);
    }
}
