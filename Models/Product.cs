using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HypotheticalRetailStore.Models;

public class Product
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string SKU { get; set; }

    public string Category { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public int StockQuantity { get; set; }

    // Foreign key to Supplier
    [Required]
    public Guid SupplierId { get; set; }

    public Supplier Supplier { get; set; } // Navigation property
}

public class ProductResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string SKU { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public Guid SupplierId { get; set; }
    public string SupplierName { get; set; }
}

