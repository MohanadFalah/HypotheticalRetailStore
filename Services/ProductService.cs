using System;
using HypotheticalRetailStore.DATA;
using HypotheticalRetailStore.DTOs;
using HypotheticalRetailStore.Helper;
using HypotheticalRetailStore.Interfaces;
using HypotheticalRetailStore.Models;
using Microsoft.EntityFrameworkCore;
using Nelibur.ObjectMapper;

namespace HypotheticalRetailStore.Services;

public class ProductService : IProductService
{
    private readonly InventoryDbContext _dbContext;

    public ProductService(InventoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IResult> AddProductAsync(DTOProduct dtoproduct)
    {
        if (dtoproduct is null)
            return Results.BadRequest();
    
        bool isValid;
        var validationResults = ValidationHelper.ValidateModel(dtoproduct, out isValid);
        if (!isValid)
        {
            var errorMessages = validationResults.Select(vr => vr.ErrorMessage).ToList();
            return Results.BadRequest(new { Errors = errorMessages });
        }
    
        var supplier = await _dbContext.SuppliersTable.FindAsync(dtoproduct.SupplierId);
        if (supplier == null)
            return Results.BadRequest("Supplier not found.");
    
        var product = TinyMapper.Map<Product>(dtoproduct);
        product.Id = Guid.NewGuid();
    
        await _dbContext.ProductsTable.AddAsync(product);
        await _dbContext.SaveChangesAsync();

        return Results.Ok(200);
    }



    public async Task<IResult> UpdateProductAsync(DTOProduct dtoproduct,Guid id)
    {
        if (dtoproduct is null)
            return Results.BadRequest();
        
        bool isValid;
        var validationResults = ValidationHelper.ValidateModel(dtoproduct, out isValid);
        if (!isValid)
        {
            var errorMessages = validationResults.Select(vr => vr.ErrorMessage).ToList();
            return Results.BadRequest(new { Errors = errorMessages });
        }
        
        var supplier = await _dbContext.SuppliersTable.FindAsync(dtoproduct.SupplierId);
        if (supplier == null)
            return Results.BadRequest("Supplier not found.");
        
        var existingProduct = await _dbContext.ProductsTable.FindAsync(id);
        if (existingProduct == null)
            return Results.NotFound("Product not found.");
        existingProduct.Name = dtoproduct.Name;
        existingProduct.SKU = dtoproduct.SKU;
        existingProduct.Category = dtoproduct.Category;
        existingProduct.Price = dtoproduct.Price;
        existingProduct.StockQuantity = dtoproduct.StockQuantity;
        existingProduct.SupplierId = dtoproduct.SupplierId;
        await _dbContext.SaveChangesAsync();
        return Results.Ok(200);
    }


    public async Task<IResult> DeleteProductAsync(Guid id)
    {
        var product = await _dbContext.ProductsTable.FindAsync(id);
        if (product == null)
            return Results.NotFound("Product not found.");

        _dbContext.ProductsTable.Remove(product);
        await _dbContext.SaveChangesAsync();
        return Results.Ok("Product deleted successfully.");
    }
    
    public async Task<IResult> GetProductByIdAsync(Guid id)
    {
        var product = await _dbContext.ProductsTable
            .Include(p => p.Supplier)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            return Results.NotFound("Product not found.");

        // Map to ProductDTO
        var productResponse = new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            SKU = product.SKU,
            Category = product.Category,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            SupplierId = product.Supplier.Id,
            SupplierName = product.Supplier.Name
        };

        return Results.Ok(productResponse);
    }
  
    public async Task<IResult> GetAllProductsAsync()
    {
        var products = await _dbContext.ProductsTable
            .AsNoTracking()
            .Include(p => p.Supplier)
            .Select(p => new ProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                SKU = p.SKU,
                Category = p.Category,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                SupplierId = p.Supplier.Id,
                SupplierName = p.Supplier.Name
            })
            .ToListAsync();
        return Results.Ok(products);
    }
}
