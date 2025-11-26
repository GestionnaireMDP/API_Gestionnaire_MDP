namespace Gestionnaire_MDP.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string EntraId { get; set; } = null!;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }
    public bool IsActive { get; set; } = true;
}