using System.ComponentModel.DataAnnotations;

namespace HypotheticalRetailStore.DTOs;

public class DTOLogin
{
    [Required]
    public string username { get; set; }
    
    [Required]
    public string password { get; set; }
}