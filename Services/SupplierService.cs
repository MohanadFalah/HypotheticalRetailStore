using System;
using HypotheticalRetailStore.DATA;
using HypotheticalRetailStore.DTOs;
using HypotheticalRetailStore.Helper;
using HypotheticalRetailStore.Interfaces;
using HypotheticalRetailStore.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Nelibur.ObjectMapper;

namespace HypotheticalRetailStore.Services;

public class SupplierService : ISupplierService
{
    private readonly InventoryDbContext _dbContext;

    public SupplierService(InventoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IResult> AddSupplierAsync(DTOSupplier dtosupplier)
    {
        if (dtosupplier is null)
            return Results.BadRequest();

        bool isValid;
        var validationResults = ValidationHelper.ValidateModel(dtosupplier, out isValid);
        if (!isValid)
        {
            // Convert validation results to a list of error messages
            var errorMessages = validationResults.Select(vr => vr.ErrorMessage).ToList();
            return Results.BadRequest(new { Errors = errorMessages });
        }
        
        var supplier = TinyMapper.Map<Supplier>(dtosupplier);
        supplier.Id = Guid.NewGuid();
        await _dbContext.SuppliersTable.AddAsync(supplier);
        await _dbContext.SaveChangesAsync();
        return Results.Created($"/api/suppliers/{supplier.Id}", supplier);
    }
    
    public async Task<IResult> UpdateSupplierAsync(Guid id, DTOSupplier dtosupplier)
    {
        var existingSupplier = await _dbContext.SuppliersTable.FindAsync(id);
        if (existingSupplier == null)
            return Results.NotFound("Supplier not found.");
    
        if (dtosupplier == null)
            return Results.BadRequest();


        bool isValid;
        var validationResults = ValidationHelper.ValidateModel(dtosupplier, out isValid);
        if (!isValid)
        {
            // Convert validation results to a list of error messages
            var errorMessages = validationResults.Select(vr => vr.ErrorMessage).ToList();
            return Results.BadRequest(new { Errors = errorMessages });
        }
    
   
        existingSupplier.Name = dtosupplier.Name;
        existingSupplier.MobileNumber = dtosupplier.MobileNumber;
        existingSupplier.Address = dtosupplier.Address;
        //
        _dbContext.SuppliersTable.Update(existingSupplier);
        await _dbContext.SaveChangesAsync();
        //
        return Results.Ok(existingSupplier);
    }
    public async Task<IResult> DeleteSupplierAsync(Guid id)
    {
        var supplier = await _dbContext.SuppliersTable
            .Include(s => s.Products) // Include related products
            .FirstOrDefaultAsync(s => s.Id == id);

        if (supplier == null)
            return Results.NotFound("Supplier not found.");

        if (supplier.Products.Any())
            return Results.BadRequest("Cannot delete supplier with associated products.");

        _dbContext.SuppliersTable.Remove(supplier);
        await _dbContext.SaveChangesAsync();
        return Results.Ok("Supplier deleted successfully.");
    }
    
    public async Task<IResult> GetPagination(int pageSize)
    {
        var totalItems =
            await _dbContext.SuppliersTable.AsNoTracking().CountAsync();
        return Results.Ok(new PaginationInfo
        {
            TotalPages = (int)Math.Ceiling((double)totalItems / (double)pageSize)
        });
    }

    public async Task<IResult> GetAllSuppliers(int pageNumber, int pageSize, string txtsearch)
    {

        IQueryable<Supplier> query = _dbContext.SuppliersTable;

        if (!String.IsNullOrWhiteSpace(txtsearch))
        {

            query = query.Where(x =>
                x.Name.Contains(txtsearch)
                || x.MobileNumber.Contains(txtsearch)
            );
        }
        var results= await query.Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
        
        return Results.Ok((results));
    }
    

    public async Task<IResult> GetSupplierByIdAsync(Guid id)
    {
        var supplier = await _dbContext.SuppliersTable
            .Include(s => s.Products)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);

        if (supplier == null)
            return Results.NotFound("Supplier not found.");
        var supplierResponse = new SupplierResponse
        {
            Id = supplier.Id,
            Name = supplier.Name,
            MobileNumber = supplier.MobileNumber,
            Address = supplier.Address,
            Products = supplier.Products.Select(product => new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                SKU = product.SKU,
                Category = product.Category,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                SupplierId = product.SupplierId,
                SupplierName = supplier.Name 
            }).ToList()
        };
        return Results.Ok(supplierResponse);
    }

    
    public async Task<IResult> GetAllSuppliersWithProductsAsync()
    {
        var suppliers = await _dbContext.SuppliersTable
            .Include(s => s.Products)
            .AsNoTracking()
            .ToListAsync();

        var supplierDTOs = suppliers.Select(supplier => new SupplierResponse()
        {
            Id = supplier.Id,
            Name = supplier.Name,
            MobileNumber = supplier.MobileNumber,
            Address = supplier.Address,
            Products = supplier.Products.Select(product => new ProductResponse()
            {
                Id = product.Id,
                Name = product.Name,
                SKU = product.SKU,
                Category = product.Category,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                SupplierId = product.SupplierId
            }).ToList()
        }).ToList();
        return Results.Ok(supplierDTOs);
    }
    
}
