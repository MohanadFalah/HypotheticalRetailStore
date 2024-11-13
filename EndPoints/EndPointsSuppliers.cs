using HypotheticalRetailStore.DTOs;
using HypotheticalRetailStore.Interfaces;
using HypotheticalRetailStore.Models;

namespace HypotheticalRetailStore.EndPoints;

public static class EndPointsSuppliers
{
    public static void ConfigureEndPointsSuppliers(this WebApplication app)
    {
        var endpoints = app.MapGroup("api/v1/suppliers/").WithTags("Suppliers-Endpoints");
        
        endpoints.MapPost("add-new",
            async (DTOSupplier dtosupplier, ISupplierService supplierService) =>
                await supplierService.AddSupplierAsync(dtosupplier)).RequireAuthorization("Admin");

        endpoints.MapPut("update",
                async (Guid id, DTOSupplier dtosupplier, ISupplierService supplierService) =>
                    await supplierService.UpdateSupplierAsync(id, dtosupplier))
            .RequireAuthorization("Admin");
        

        endpoints.MapDelete("delete",
                async (Guid id, ISupplierService supplierService) =>
                    await supplierService.DeleteSupplierAsync(id))
            .RequireAuthorization("Admin");
        //
        endpoints.MapGet("get-pagination",
            async (int pageSize, ISupplierService supplierService) =>
                await supplierService.GetPagination(pageSize)).RequireAuthorization();
        
        endpoints.MapGet("get-all",
            async (int pageNumber, int pageSize, string txtsearch, ISupplierService supplierService) =>
                await supplierService.GetAllSuppliers(pageNumber, pageSize, txtsearch)).RequireAuthorization();
        
        endpoints.MapGet("view",
            async (Guid id, ISupplierService supplierService) =>
                await supplierService.GetSupplierByIdAsync(id)).RequireAuthorization();
        
        endpoints.MapGet("get-all-with-products",
                async (ISupplierService supplierService) =>
                    await supplierService.GetAllSuppliersWithProductsAsync())
            .RequireAuthorization();
      
    }
}