namespace Stride.Domain.Entities;

public class User : BaseAuditableEntity<string>
{
    public List<TodoList> TodoLists { get; set; } = [];
}