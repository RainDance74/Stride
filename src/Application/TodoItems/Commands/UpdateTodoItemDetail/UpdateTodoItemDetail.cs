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
        Domain.Entities.TodoItem? entity = await _context.TodoItems
            .Include(i => i.TodoList)
            .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        Guard.Against.Null(_user.Id);

        if(entity.TodoList.Owner != _user.Id)
        {
            throw new UnauthorizedAccessException("User should be owner of the target list.");
        }

        entity.Description = request.Description;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
