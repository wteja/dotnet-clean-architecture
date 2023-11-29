namespace Domain.Common;

public class BaseEntity
{
    public virtual int Id { get; set; }
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }
    public bool? IsActive { get; set; }
}