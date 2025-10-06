using Domain.Entities.Auth;

namespace Domain.Entities;

public class UserMember : BaseEntity
{
    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    // relaciones uno a muchos
    public ICollection<UserMemberRol> UserMemberRoles { get; set; } = new List<UserMemberRol>();
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
