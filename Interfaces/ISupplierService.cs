using HypotheticalRetailStore.DTOs;
using HypotheticalRetailStore.Models;

namespace HypotheticalRetailStore.Interfaces;

public interface ISupplierService
{
    Task<IResult> AddSupplierAsync(DTOSupplier dtosupplier);
    Task<IResult> UpdateSupplierAsync(Guid id, DTOSupplier dtosupplier);
    Task<IResult> DeleteSupplierAsync(Guid id);
    Task<IResult> GetPagination(int pageSize);
    Task<IResult> GetAllSuppliers(int pageNumber, int pageSize, string txtsearch);
    Task<IResult> GetSupplierByIdAsync(Guid id);
    Task<IResult> GetAllSuppliersWithProductsAsync();
}