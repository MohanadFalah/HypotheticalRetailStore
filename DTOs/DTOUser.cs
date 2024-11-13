using System.ComponentModel.DataAnnotations;

namespace HypotheticalRetailStore.DTOs;

public class DTOUser
{
    [Required(ErrorMessage = "Username is required.")]
    [MaxLength(30, ErrorMessage = "Username cannot exceed 30 characters.")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    public string PasswordHash { get; set; }

    [Required(ErrorMessage = "Role is required.")]
    [RegularExpression("^(Admin|User)$", ErrorMessage = "Role must be either 'Admin' or 'User'.")]
    public string Role { get; set; }
}
