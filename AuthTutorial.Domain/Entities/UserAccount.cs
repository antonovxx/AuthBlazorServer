using Domain.Enums;

namespace Domain.Entities;

public class UserAccount : BaseEntity
{
    public string Name { get; set; }
    
    public string Password { get; set; }
    
    public RoleType Role { get; set; }
}