using Stride.Domain.Entities;

namespace Stride.Application.TodoLists.Queries.GetTodoLists;

public class TodoItemDto
{
    public int Id { get; init; }

    public int ListId { get; init; }

    public string? Title { get; init; }

    public bool Done { get; init; }

    public string? Description { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<TodoItem, TodoItemDto>()
                .ForMember(i => i.ListId, 
                opt => opt.MapFrom(s => s.TodoList.Id));
        }
    }
}