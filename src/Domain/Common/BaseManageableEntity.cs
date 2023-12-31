namespace Stride.Domain.Common;

public class BaseManageableEntity : BaseAuditableEntity<int>
{
    public StrideUser CreatedBy { get; set; } = null!;
    public StrideUser? UpdatedBy { get; set; }
}
