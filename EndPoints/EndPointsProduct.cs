using HypotheticalRetailStore.DTOs;
using HypotheticalRetailStore.Interfaces;

namespace HypotheticalRetailStore.EndPoints;

public static class EndPointsProduct
{
    public static void ConfigureEndPointsProduct(this WebApplication app)
    {
        var endpoints = app.MapGroup("api/v1/products/").WithTags("Products-Endpoints");

        endpoints.MapPost("add-new",
                async (IProductService service, DTOProduct dtoproduct) =>
                    await service.AddProductAsync(dtoproduct))
            .RequireAuthorization();

        endpoints.MapPut("update",
                async (Guid id, IProductService service, DTOProduct dtoproduct) =>
                    await service.UpdateProductAsync(dtoproduct, id))
            .RequireAuthorization();


        endpoints.MapDelete("delete",
                async (Guid id, IProductService service) =>
                    await service.DeleteProductAsync(id))
            .RequireAuthorization();


        endpoints.MapGet("get-all",
            async (IProductService service) =>
                await service.GetAllProductsAsync()).RequireAuthorization();

        endpoints.MapGet("get-one",
            async (IProductService service, Guid id) =>
                await service.GetProductByIdAsync(id)).RequireAuthorization();
    }
}