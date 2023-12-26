namespace Stride.Domain.Entities;

public class TodoItem : BaseAuditableEntity<int>
{
    private bool _done;

    public string? Title { get; set; }

    public string? Description { get; set; }

    public IUser<string>? Creator { get; set; }

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