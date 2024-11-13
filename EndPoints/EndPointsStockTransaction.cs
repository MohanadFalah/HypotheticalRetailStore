using HypotheticalRetailStore.DTOs;
using HypotheticalRetailStore.Interfaces;

namespace HypotheticalRetailStore.EndPoints;

public static class EndPointsStockTransaction
{
    public static void ConfigureEndPointsStockTransaction(this WebApplication app)
    {
        var endpoints = app.MapGroup("api/v1/stocktransactions/")
            .WithTags("StockTransactions-Endpoints");
        
        endpoints.MapPost("add-new",
                async (IStockTransactionService service, DTOStockTransaction transactionDto) =>
                    await service.AddTransactionAsync(transactionDto))
            .RequireAuthorization();

        endpoints.MapGet("get-one",
                async (IStockTransactionService service, Guid transactionId) =>
                    await service.GetTransactionByIdAsync(transactionId))
            .RequireAuthorization();
     
        endpoints.MapGet("get-product-transactions",
                async (IStockTransactionService service, Guid productId) =>
                    await service.GetTransactionsByProductIdAsync(productId))
            .RequireAuthorization();
        
        
        endpoints.MapGet("get-pagination",
            async (int pageSize, IStockTransactionService service) =>
                await service.GetPagination(pageSize)).RequireAuthorization();
        
        endpoints.MapGet("getall",
                async (IStockTransactionService service, int pageNumber = 1, int pageSize = 10) =>
                    await service.GetAllTransactionsAsync(pageNumber, pageSize))
            .RequireAuthorization();

       
        endpoints.MapGet("get-between-dates",
                async (IStockTransactionService service,long unixStartDate , long unixEndDate ) =>
                    await service.GetTransactionsByDateRangeAsync(unixStartDate, unixEndDate))
            .RequireAuthorization();
        
        
        endpoints.MapPut("update",
                async (IStockTransactionService service, Guid transactionId, DTOStockTransaction transactionDto) =>
                    await service.UpdateTransactionAsync(transactionId, transactionDto))
            .RequireAuthorization();
        
        endpoints.MapDelete("delete",
                async (IStockTransactionService service, Guid transactionId) =>
                    await service.DeleteTransactionAsync(transactionId))
            .RequireAuthorization();
    }

}