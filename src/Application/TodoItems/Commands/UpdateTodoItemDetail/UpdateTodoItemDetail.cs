using Stride.Application.Common.Interfaces;

namespace Stride.Application.TodoItems.Commands.UpdateTodoItemDetail;

public record UpdateTodoItemDetailCommand : IRequest
{
    public int Id { get; init; }

    public string? Description { get; init; }
}

public class UpdateTodoItemDetailCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateTodoItemDetailCommand>
{
    private readonly IApplicationDbContext _context = context;

    public async Task Handle(UpdateTodoItemDetailCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoItems
            .FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Description = request.Description;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
