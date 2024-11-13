using HypotheticalRetailStore.DATA;
using HypotheticalRetailStore.DTOs;
using HypotheticalRetailStore.Helper;
using HypotheticalRetailStore.Interfaces;
using HypotheticalRetailStore.Models;
using Microsoft.EntityFrameworkCore;

namespace HypotheticalRetailStore.Services;

public class AuthService:IAuthService
{
    private readonly InventoryDbContext _dbContext;

    public AuthService(InventoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IResult> Authenticate(DTOLogin dtologin)
    {
        if (dtologin is null)
            return Results.BadRequest();
        bool isValid;
        var validationResults = ValidationHelper.ValidateModel(dtologin, out isValid);
        if (!isValid)
        {
            var errorMessages = validationResults.Select(vr => vr.ErrorMessage).ToList();
            return Results.BadRequest(new { Errors = errorMessages });
        }
        var user = await _dbContext.UsersTable.FirstOrDefaultAsync(u => u.Username == dtologin.username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(dtologin.password, user.PasswordHash))
            return Results.Unauthorized();

        var expirationDate = DateTime.UtcNow.AddHours(1);
        long expirationToken = new DateTimeOffset(expirationDate).ToUnixTimeSeconds();
        
        var response = new TokenResponse()
        {
            UserId = user.Id,
            TokenData = TokenGenerator.GenerateToken(expirationDate,user.Id,user.Role),
            Expire = expirationToken
        };
        return Results.Ok(response);
    }
    
}