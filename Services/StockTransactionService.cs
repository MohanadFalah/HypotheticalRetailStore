using HypotheticalRetailStore.Interfaces;
using HypotheticalRetailStore.DATA;
using HypotheticalRetailStore.DTOs;
using HypotheticalRetailStore.Helper;
using HypotheticalRetailStore.Interfaces;
using HypotheticalRetailStore.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Nelibur.ObjectMapper;
namespace HypotheticalRetailStore.Services;

public class StockTransactionService : IStockTransactionService
{

    private readonly InventoryDbContext _dbContext;

    public StockTransactionService(InventoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IResult> AddTransactionAsync(DTOStockTransaction transactionDto)
    {

        if (transactionDto is null)
            return Results.BadRequest();

        bool isValid;
        var validationResults = ValidationHelper.ValidateModel(transactionDto, out isValid);
        if (!isValid)
        {
            var errorMessages = validationResults.Select(vr => vr.ErrorMessage).ToList();
            return Results.BadRequest(new { Errors = errorMessages });
        }
        //

        var product = await _dbContext.ProductsTable.FindAsync(transactionDto.ProductId);
        if (product == null)
            return Results.NotFound("Product not found");

        if (transactionDto.Type == "Addition")
        {
            product.StockQuantity += transactionDto.Quantity;
        }

        else if (transactionDto.Type == "Removal")
        {
            if (product.StockQuantity < transactionDto.Quantity)
                return Results.BadRequest("Insufficient stock");
            product.StockQuantity -= transactionDto.Quantity;
        }
        else
        {
            return Results.BadRequest("Invalid type");
        }


        var stockTransaction = TinyMapper.Map<StockTransaction>(transactionDto);

        stockTransaction.Id = Guid.NewGuid();
        stockTransaction.TransactionDate = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        

        _dbContext.StockTransactionsTable.Add(stockTransaction);
        await _dbContext.SaveChangesAsync();

        return Results.Ok(stockTransaction.Id);
    }

    
    public async Task<IResult> GetPagination(int pageSize)
    {
        var totalItems =
            await _dbContext.StockTransactionsTable.AsNoTracking().CountAsync();
        return Results.Ok(new PaginationInfo
        {
            TotalPages = (int)Math.Ceiling((double)totalItems / (double)pageSize)
        });
    }
    
    public async Task<IResult> GetTransactionByIdAsync(Guid transactionId)
    {
        var transaction = await _dbContext.StockTransactionsTable
            .AsNoTracking().FirstOrDefaultAsync(t => t.Id == transactionId);
        if (transaction == null)
        {
            return Results.NotFound("Transaction not found.");
        }
        return Results.Ok(transaction);
    }

    public async Task<IResult> GetTransactionsByProductIdAsync(Guid productId)
    {
        var transactions = await _dbContext.StockTransactionsTable
            .AsNoTracking()
            .Where(t => t.ProductId == productId)
            .Include(t => t.Product) 
            .Select(t => new StockTransactionResponse
            {
                Id = t.Id,
                ProductId = t.ProductId,
                Quantity = t.Quantity,
                TransactionDate = DateTimeOffset.FromUnixTimeSeconds(t.TransactionDate).UtcDateTime,
                Type = t.Type,
                Product = new ProductResponse
                {
                    Id = t.Product.Id,
                    Name = t.Product.Name,
                    SKU = t.Product.SKU,
                    Category = t.Product.Category,
                    Price = t.Product.Price,
                    StockQuantity = t.Product.StockQuantity,
                    SupplierId = t.Product.SupplierId,
                    SupplierName = t.Product.Supplier.Name 
                }
            })
            .ToListAsync();

        if (!transactions.Any())
        {
            return Results.NotFound("No transactions found for this product.");
        }

        return Results.Ok(transactions);
    }

    public async Task<IResult> GetAllTransactionsAsync(int pageNumber, int pageSize)
    {
        var transactions = await _dbContext.StockTransactionsTable.AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize).ToListAsync();
        return Results.Ok(transactions);
    }

    public async Task<IResult> UpdateTransactionAsync(Guid transactionId, DTOStockTransaction transactionDto)
    {
        if (transactionDto is null)
            return Results.BadRequest();


        bool isValid;
        var validationResults = ValidationHelper.ValidateModel(transactionDto, out isValid);
        if (!isValid)
        {
            var errorMessages = validationResults.Select(vr => vr.ErrorMessage).ToList();
            return Results.BadRequest(new { Errors = errorMessages });
        }

        var transaction = await _dbContext.StockTransactionsTable.FindAsync(transactionId);
        if (transaction == null)
            return Results.NotFound("Transaction not found.");

        var product = await _dbContext.ProductsTable.FindAsync(transaction.ProductId);
        if (product == null)
            return Results.NotFound("Product not found.");


        if (transaction.Type == "Addition")
            product.StockQuantity -= transaction.Quantity;
        else if (transaction.Type == "Removal")
            product.StockQuantity += transaction.Quantity;

        // appy new trasaction
        if (transactionDto.Type == "Addition")
        {
            product.StockQuantity += transactionDto.Quantity;
        }
        else if (transactionDto.Type == "Removal")
        {
            if (product.StockQuantity < transactionDto.Quantity)
                return Results.BadRequest("Insufficient stock.");
            product.StockQuantity -= transactionDto.Quantity;
        }
        else
        {
            return Results.BadRequest("Invalid type.");
        }

        // Update transaction details
        transaction.ProductId = transactionDto.ProductId;
        transaction.Quantity = transactionDto.Quantity;
        transaction.TransactionDate = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        transaction.Type = transactionDto.Type;
        await _dbContext.SaveChangesAsync();
        return Results.Ok("Transaction updated successfully.");
    }

    public async Task<IResult> DeleteTransactionAsync(Guid transactionId)
    {
        var transaction = await _dbContext.StockTransactionsTable.FindAsync(transactionId);
        if (transaction == null)
            return Results.NotFound("Transaction not found.");

        var product = await _dbContext.ProductsTable.FindAsync(transaction.ProductId);
        if (product == null)
            return Results.NotFound("Product not found.");

        //revert
        if (transaction.Type == "Addition")
            product.StockQuantity -= transaction.Quantity;
        else if (transaction.Type == "Removal")
            product.StockQuantity += transaction.Quantity;

        _dbContext.StockTransactionsTable.Remove(transaction);
        await _dbContext.SaveChangesAsync();

        return Results.Ok("Transaction deleted successfully.");
    }
    
    public async Task<IResult> GetTransactionsByDateRangeAsync(long unixStartDate , long unixEndDate )
    {
        // Convert startDate and endDate to Unix time seconds
        // long unixStartDate = new DateTimeOffset(startDate.ToUniversalTime()).ToUnixTimeSeconds();
        // long unixEndDate = new DateTimeOffset(endDate.ToUniversalTime()).ToUnixTimeSeconds();

        var transactions = await _dbContext.StockTransactionsTable
            .AsNoTracking()
            .Where(t => t.TransactionDate >= unixStartDate && t.TransactionDate <= unixEndDate)
            .Include(t => t.Product)
            .Select(t => new StockTransactionResponse
            {
                Id = t.Id,
                ProductId = t.ProductId,
                Quantity = t.Quantity,
                TransactionDate = DateTimeOffset.FromUnixTimeSeconds(t.TransactionDate).UtcDateTime,
                Type = t.Type,
                Product = new ProductResponse
                {
                    Id = t.Product.Id,
                    Name = t.Product.Name,
                    SKU = t.Product.SKU,
                    Category = t.Product.Category,
                    Price = t.Product.Price,
                    StockQuantity = t.Product.StockQuantity,
                    SupplierId = t.Product.SupplierId,
                    SupplierName = t.Product.Supplier.Name 
                }
            })
            .ToListAsync();

        if (!transactions.Any())
        {
            return Results.NotFound("No transactions found for the specified date range.");
        }
        return Results.Ok(transactions);
    }


}