namespace Domain;

public interface IEntity
{
    long Id { get; set; }
    
    DateTime CreatedAtUtc { get; set; }
    
    DateTime UpdatedAtUtc { get; set; }
}