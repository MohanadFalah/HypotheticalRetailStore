using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HypotheticalRetailStore.Models;
public class Supplier
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string MobileNumber { get; set; }
    
    public string Address { get; set; }
    
    public ICollection<Product> Products { get; set; } = new List<Product>();
}


public class SupplierResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string MobileNumber { get; set; }
    public string Address { get; set; }
    public List<ProductResponse> Products { get; set; } = new List<ProductResponse>();
}