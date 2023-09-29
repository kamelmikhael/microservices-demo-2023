namespace Ordering.Domain.Common;

public abstract class EntityBase
{
    public int Id { get; protected set; }

    public string? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set;} = DateTime.UtcNow;

    public string? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
}
