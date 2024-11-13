using HypotheticalRetailStore.DTOs;
using HypotheticalRetailStore.Models;
namespace HypotheticalRetailStore.Interfaces;

public interface IProductService
{
    Task<IResult> AddProductAsync(DTOProduct dtoproduct);
    
    Task<IResult> UpdateProductAsync(DTOProduct dtoproduct,Guid id);
    Task<IResult> DeleteProductAsync(Guid id);
    Task<IResult> GetProductByIdAsync(Guid id);
    Task<IResult> GetAllProductsAsync();
}