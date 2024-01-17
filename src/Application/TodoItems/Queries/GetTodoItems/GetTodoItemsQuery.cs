using Stride.Application.Common.Interfaces;

namespace Stride.Application.TodoItems.Queries.GetTodoItems;
public record GetTodoItemsQuery(int Id) : IRequest<TodoItemsVm>;

public class GetTodoItemsQueryHandler(IApplicationDbContext context, IMapper mapper, IUser user)
    : IRequestHandler<GetTodoItemsQuery, TodoItemsVm>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;
    private readonly IUser _user = user;

    public async Task<TodoItemsVm> Handle(GetTodoItemsQuery request, CancellationToken cancellationToken)
    {
        return new TodoItemsVm
        {
            Items = await _context.TodoItems
                .AsNoTracking()
                .Where(i => i.TodoList.Owner == _user.Id)
                .Where(i => i.TodoList.Id == request.Id)
                .ProjectTo<TodoItemDto>(_mapper.ConfigurationProvider)
                .OrderByDescending(l => l.Id)
                .ToListAsync(cancellationToken)
        };
    }
}