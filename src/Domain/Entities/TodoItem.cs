namespace Stride.Domain.Entities;

public class TodoItem : BaseManageableEntity
{
    private bool _done;

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public TodoList TodoList { get; set; } = null!;

    public bool Done
    {
        get => _done;
        set
        {
            if(value && !_done)
            {
                AddDomainEvent(new TodoItemCompletedEvent(this));
            }

            _done = value;
        }
    }
}