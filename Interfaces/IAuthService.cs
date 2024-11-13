using HypotheticalRetailStore.DTOs;

namespace HypotheticalRetailStore.Interfaces;

public interface IAuthService
{
    Task<IResult> Authenticate(DTOLogin dtologin);
    
}