using HypotheticalRetailStore.DTOs;

namespace HypotheticalRetailStore.Interfaces;

public interface IStockTransactionService
{
    
    Task<IResult> AddTransactionAsync(DTOStockTransaction transactionDto);

    Task<IResult> GetPagination(int pageSize);
    
    Task<IResult> GetTransactionByIdAsync(Guid transactionId);
    Task<IResult> GetTransactionsByProductIdAsync(Guid productId);

    Task<IResult> GetAllTransactionsAsync(int pageNumber, int pageSize);

    Task<IResult> UpdateTransactionAsync(Guid transactionId, DTOStockTransaction transactionDto);
    
    Task<IResult> DeleteTransactionAsync(Guid transactionId);
    
    Task<IResult> GetTransactionsByDateRangeAsync(long unixStartDate , long unixEndDate );
}

