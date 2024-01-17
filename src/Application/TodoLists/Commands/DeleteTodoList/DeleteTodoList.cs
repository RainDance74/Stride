using Stride.Application.Common.Interfaces;

namespace Stride.Application.TodoLists.Commands.DeleteTodoList;

public record DeleteTodoListCommand(int Id) : IRequest;

public class DeleteTodoListCommandHandler(IApplicationDbContext context, IUser user) : IRequestHandler<DeleteTodoListCommand>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IUser _user = user;

    public async Task Handle(DeleteTodoListCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.TodoList? entity = await _context.TodoLists
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        Guard.Against.Null(_user.Id);

        if(entity.Owner != _user.Id)
        {
            throw new UnauthorizedAccessException("User should be owner of the list.");
        }

        _context.TodoLists.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
