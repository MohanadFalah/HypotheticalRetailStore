using HypotheticalRetailStore.DTOs;

namespace HypotheticalRetailStore.Interfaces;

public interface IUserService
{
    Task<IResult> AddNewUser(DTOUser dtouser);
    
    Task<IResult> UpdateUser(DTOUser dtouser,Guid guid);
    
    Task<IResult> DeleteUser(Guid guid);
    
    Task<IResult> GetAllUsers();
}


//  Task<IResult> AuthenticateUserAsync(string username, string password);