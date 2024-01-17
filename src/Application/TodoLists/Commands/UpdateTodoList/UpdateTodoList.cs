using Stride.Application.Common.Interfaces;

namespace Stride.Application.TodoLists.Commands.UpdateTodoList;

public record UpdateTodoListCommand : IRequest
{
    public int Id { get; init; }

    public string? Title { get; init; }
}

public class UpdateTodoListCommandHandler(IApplicationDbContext context, IUser user) : IRequestHandler<UpdateTodoListCommand>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IUser _user = user;

    public async Task Handle(UpdateTodoListCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.TodoList? entity = await _context.TodoLists
            .FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        Guard.Against.Null(_user.Id);

        if(entity.Owner != _user.Id)
        {
            throw new UnauthorizedAccessException("User should be owner of the list.");
        }

        entity.Title = request.Title;

        await _context.SaveChangesAsync(cancellationToken);
    }
}