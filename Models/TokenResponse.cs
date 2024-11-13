namespace HypotheticalRetailStore.Models;

public class TokenResponse
{
    public Guid UserId { get; set; }
    public string TokenData { get; set; }
    public long Expire { get; set; }
}