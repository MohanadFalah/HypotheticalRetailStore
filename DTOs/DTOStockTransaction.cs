using System.ComponentModel.DataAnnotations;

namespace HypotheticalRetailStore.DTOs;

public class DTOStockTransaction
{
    [Required]
    public Guid ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive integer.")]
    public int Quantity { get; set; }

    [Required]
    [RegularExpression("^(Addition|Removal)$", ErrorMessage = "Type must be either 'Addition' or 'Removal'.")]
    public string Type { get; set; }
}