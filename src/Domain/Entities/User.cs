namespace Stride.Domain.Entities;

public class StrideUser : BaseAuditableEntity<string>
{
    public List<TodoList> TodoLists { get; set; } = [];
}