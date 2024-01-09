namespace Stride.Application.TodoLists.Queries.GetTodoLists;
public class TodoListsVm
{
    public IReadOnlyCollection<TodoListDto> Lists { get; init; } = [];
}
