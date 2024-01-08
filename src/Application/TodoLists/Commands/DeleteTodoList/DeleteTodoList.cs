using Stride.Application.Common.Interfaces;

namespace Stride.Application.TodoLists.Commands.DeleteTodoList;

public record DeleteTodoListCommand(int Id) : IRequest;

public class DeleteTodoListCommandHandler(IApplicationDbContext context, IUser user) : IRequestHandler<DeleteTodoListCommand>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IUser _user = user;

    public async Task Handle(DeleteTodoListCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoLists
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        Guard.Against.Null(_user.Id);

        var currentUser = await _context.StrideUsers
            .FindAsync(new object[] { _user.Id }, cancellationToken);

        Guard.Against.NotFound(_user.Id, currentUser);

        if(entity.Owner != currentUser)
        {
            throw new UnauthorizedAccessException("User should be owner of the list.");
        }

        _context.TodoLists.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
