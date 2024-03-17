namespace Domain;

public class BaseEntity : IEntity
{
    public long Id { get; set; }
    
    public DateTime CreatedAtUtc { get; set; }
    
    public DateTime UpdatedAtUtc { get; set; }
}