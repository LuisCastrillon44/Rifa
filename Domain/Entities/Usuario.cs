using Domain.Common;

namespace Domain.Entities;

public class Usuario : BaseEntity
{
    public string? Name { get; set; }
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string? Phone { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<Talonario> Talonarios { get; set; } = new List<Talonario>();
}
