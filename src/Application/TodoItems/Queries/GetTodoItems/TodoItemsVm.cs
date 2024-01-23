namespace Stride.Application.TodoItems.Queries.GetTodoItems;

public class TodoItemsVm
{
    public IReadOnlyCollection<TodoItemDto> Items { get; init; } = [];
}
