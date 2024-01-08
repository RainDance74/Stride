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
        var entity = await _context.TodoLists
            .FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        Guard.Against.Null(_user.Id);

        var currentUser = await _context.StrideUsers
            .FindAsync(new object[] { _user.Id }, cancellationToken);

        Guard.Against.NotFound(_user.Id, currentUser);

        if (entity.Owner != currentUser)
        {
            throw new UnauthorizedAccessException("User should be owner of the list.");
        }

        entity.Title = request.Title;

        await _context.SaveChangesAsync(cancellationToken);
    }
}