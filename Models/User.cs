using System.ComponentModel.DataAnnotations;

namespace HypotheticalRetailStore.Models;

public class User
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string PasswordHash { get; set; } 

    [Required]
    public string Role { get; set; } 
}
