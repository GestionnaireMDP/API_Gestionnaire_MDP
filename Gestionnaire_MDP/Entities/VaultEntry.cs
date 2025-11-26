namespace Gestionnaire_MDP.Entities;

public class VaultEntry
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid VaultId { get; set; }
    public Vault Vault { get; set; } = null!;

    public byte[] EncryptedData { get; set; } = null!;
    public byte[] EncryptionIv { get; set; } = null!;
    public byte[] EncryptionTag { get; set; } = null!;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }
    public DateTime? DeletedAtUtc { get; set; }
}