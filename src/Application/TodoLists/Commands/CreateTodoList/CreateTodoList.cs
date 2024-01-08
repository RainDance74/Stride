using Stride.Application.Common.Interfaces;
using Stride.Domain.Entities;

namespace Stride.Application.TodoLists.Commands.CreateTodoList;

public record CreateTodoListCommand : IRequest<int>
{
    public string? Title { get; init; }
}

public class CreateTodoListCommandHandler(IApplicationDbContext context, IUser user) : IRequestHandler<CreateTodoListCommand, int>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IUser _user = user;

    public async Task<int> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(_user.Id);

        var user = await _context.StrideUsers
            .FindAsync(new object[] { _user.Id }, cancellationToken);

        Guard.Against.Null(user);

        var entity = new TodoList
        {
            Title = request.Title,
            Owner = user
        };

        _context.TodoLists.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
