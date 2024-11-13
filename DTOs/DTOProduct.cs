using System.ComponentModel.DataAnnotations;
using HypotheticalRetailStore.Models;

namespace HypotheticalRetailStore.DTOs;

public class DTOProduct
{
    [Required(ErrorMessage = "Product name is required.")]
    [MaxLength(50, ErrorMessage = "Product name cannot exceed 50 characters.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "SKU is required.")]
    [MaxLength(20, ErrorMessage = "SKU cannot exceed 20 characters.")]
    public string SKU { get; set; }

    [Required(ErrorMessage = "Category is required.")]
    [MaxLength(30, ErrorMessage = "Category cannot exceed 30 characters.")]
    public string Category { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Stock quantity is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Stock quantity must be 0 or greater.")]
    public int StockQuantity { get; set; }

    [Required(ErrorMessage = "Supplier ID is required.")]
    public Guid SupplierId { get; set; }
}
