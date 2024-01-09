using Stride.Domain.Entities;

namespace Stride.Application.TodoLists.Queries.GetTodoLists;
public class TodoListDto
{
    public TodoListDto()
    {
        Items = Array.Empty<TodoItemDto>();
    }

    public int Id { get; init; }

    public string? Title { get; init; }

    public IReadOnlyCollection<TodoItemDto> Items { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<TodoList, TodoListDto>()
                .ForMember(d => d.Items, 
                opt => opt.MapFrom(src => src.Items.OrderByDescending(i => i.Id)));;
        }
    }
}
