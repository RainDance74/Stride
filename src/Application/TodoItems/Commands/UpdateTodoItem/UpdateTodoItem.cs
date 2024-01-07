using Stride.Application.Common.Interfaces;

namespace Stride.Application.TodoItems.Commands.UpdateTodoItem;

public record UpdateTodoItemCommand : IRequest
{
    public int Id { get; init; }

    public string Title { get; init; } = null!;

    public bool Done { get; init; }
}

public class UpdateTodoItemCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateTodoItemCommand>
{
    private readonly IApplicationDbContext _context = context;

    public async Task Handle(UpdateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoItems
            .FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Title = request.Title;
        entity.Done = request.Done;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
