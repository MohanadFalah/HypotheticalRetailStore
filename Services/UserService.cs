using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HypotheticalRetailStore.DATA;
using HypotheticalRetailStore.DTOs;
using HypotheticalRetailStore.Helper;
using HypotheticalRetailStore.Interfaces;
using HypotheticalRetailStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Nelibur.ObjectMapper;

namespace HypotheticalRetailStore.Services;

public class UserService : IUserService
{
    private readonly InventoryDbContext _dbContext;

    public UserService(InventoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<IResult> AddNewUser(DTOUser dtouser)
    {
        bool isValid;
        var validationResults = ValidationHelper.ValidateModel(dtouser, out isValid);
        if (!isValid)
        {
            var errorMessages = validationResults.Select(vr => vr.ErrorMessage).ToList();
            return Results.BadRequest(new { Errors = errorMessages });
        }

        // Hash the password
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dtouser.PasswordHash);

        var newUser=TinyMapper.Map<User>(dtouser);
        newUser.Id = Guid.NewGuid();
        newUser.PasswordHash = passwordHash;
        
        await _dbContext.UsersTable.AddAsync(newUser);
        await _dbContext.SaveChangesAsync();
        return Results.Ok(newUser);
    }
    
    public async Task<IResult> UpdateUser(DTOUser dtouser, Guid guid)
    {
        var existingUser = await _dbContext.UsersTable.FindAsync(guid);
        if (existingUser == null)
            return Results.NotFound("User not found.");

        bool isValid;
        var validationResults = ValidationHelper.ValidateModel(dtouser, out isValid);
        if (!isValid)
        {
            var errorMessages = validationResults.Select(vr => vr.ErrorMessage).ToList();
            return Results.BadRequest(new { Errors = errorMessages });
        }
        
        existingUser.Username = dtouser.Username;
        existingUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dtouser.PasswordHash);
        existingUser.Role = dtouser.Role;
        
        _dbContext.UsersTable.Update(existingUser);
        await _dbContext.SaveChangesAsync();
        return Results.Ok(existingUser);
    }
    
    public async Task<IResult> DeleteUser(Guid guid)
    {
        var user = await _dbContext.UsersTable.FindAsync(guid);
        if (user == null)
            return Results.NotFound("User not found.");

        _dbContext.UsersTable.Remove(user);
        await _dbContext.SaveChangesAsync();
        return Results.Ok("User deleted successfully.");
    }

    public async Task<IResult> GetAllUsers()
    {
        var users = await _dbContext.UsersTable.AsNoTracking().ToListAsync();
        return Results.Ok(users);
    }

}
