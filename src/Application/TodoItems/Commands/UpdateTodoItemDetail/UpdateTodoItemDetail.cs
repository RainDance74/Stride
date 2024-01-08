using Stride.Application.Common.Interfaces;

namespace Stride.Application.TodoItems.Commands.UpdateTodoItemDetail;

public record UpdateTodoItemDetailCommand : IRequest
{
    public int Id { get; init; }

    public string? Description { get; init; }
}

public class UpdateTodoItemDetailCommandHandler(IApplicationDbContext context, IUser user) : IRequestHandler<UpdateTodoItemDetailCommand>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IUser _user = user;

    public async Task Handle(UpdateTodoItemDetailCommand request, CancellationToken cancellationToken)
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

        entity.Description = request.Description;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
