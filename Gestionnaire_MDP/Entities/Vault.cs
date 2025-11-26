namespace Gestionnaire_MDP.Entities;

public class Vault
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public string HashMdp { get; set; } = null!;
    
    // Argon2Id parameters
    public byte[] Argon2Salt { get; set; } = null!;
    public int Argon2MemoryKb { get; set; }
    public int Argon2Iterations { get; set; }
    public int Argon2Parallelism { get; set; }
    public int Argon2HashLength { get; set; }
    public int Argon2Version { get; set; }
    
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }
    public DateTime? DeletedAtUtc { get; set; }
    
    public ICollection<VaultEntry> VaultEntries { get; set; } = new List<VaultEntry>();
}