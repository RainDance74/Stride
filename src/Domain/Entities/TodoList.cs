namespace Stride.Domain.Entities;

public class TodoList : BaseManageableEntity
{
    public string? Title { get; set; }

    public StrideUser Owner { get; set; } = null!;

    public IList<TodoItem> Items { get; private set; } = [];
}
