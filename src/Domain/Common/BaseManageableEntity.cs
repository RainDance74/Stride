namespace Stride.Domain.Common;

public class BaseManageableEntity : BaseAuditableEntity<int>
{
    public string CreatedBy { get; set; } = null!;
    public string? UpdatedBy { get; set; }
}
