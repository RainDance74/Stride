namespace Stride.Domain.Common;

public class BaseManageableEntity : BaseAuditableEntity<int>
{
    public User CreatedBy { get; set; } = null!;
    public User? UpdatedBy { get; set; }
}
