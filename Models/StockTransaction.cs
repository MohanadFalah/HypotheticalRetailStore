using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices.JavaScript;

namespace HypotheticalRetailStore.Models;

public class StockTransaction
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid ProductId { get; set; }
    public Product Product { get; set; } 

    [Required]
    public int Quantity { get; set; } 

    [Required]
    [Column(TypeName = "bigint")]
    public long TransactionDate { get; set; }

    [Required]
    public string Type { get; set; }
}

public class StockTransactionResponse
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public DateTime TransactionDate { get; set; }
    public string Type { get; set; }
    public ProductResponse Product { get; set; }
}