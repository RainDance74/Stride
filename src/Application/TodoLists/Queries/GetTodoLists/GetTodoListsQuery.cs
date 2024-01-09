using Stride.Application.Common.Interfaces;

namespace Stride.Application.TodoLists.Queries.GetTodoLists;

public record GetTodoListsQuery : IRequest<TodoListsVm>;

public class GetTodoListsQueryHandler(IApplicationDbContext context, IMapper mapper, IUser user) 
    : IRequestHandler<GetTodoListsQuery, TodoListsVm>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;
    private readonly IUser _user = user;

    public async Task<TodoListsVm> Handle(GetTodoListsQuery request, CancellationToken cancellationToken)
    {
        return new TodoListsVm
        {
            Lists = await _context.TodoLists
                .AsNoTracking()
                .Where(l => l.Owner.Id == _user.Id)
                .ProjectTo<TodoListDto>(_mapper.ConfigurationProvider)
                .OrderByDescending(l => l.Id)
                .ToListAsync(cancellationToken)
        };
    }
}