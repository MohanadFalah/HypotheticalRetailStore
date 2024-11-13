using System.ComponentModel.DataAnnotations;
using HypotheticalRetailStore.Models;

namespace HypotheticalRetailStore.DTOs;

public class DTOSupplier
{
    [Required(ErrorMessage = "Name is required.")]
    [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Mobile number is required.")]
    [Phone(ErrorMessage = "Invalid mobile number format.")]
    public string MobileNumber { get; set; }

    [MaxLength(100, ErrorMessage = "Address cannot exceed 100 characters.")]
    public string Address { get; set; }
}
