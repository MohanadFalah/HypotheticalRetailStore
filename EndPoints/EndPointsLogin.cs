using HypotheticalRetailStore.DTOs;
using HypotheticalRetailStore.Interfaces;

namespace HypotheticalRetailStore.EndPoints;

public static class EndPointsLogin
{
    public static void ConfigureEndPointsLogin(this WebApplication app)
    {
        var endpoints = app.MapGroup("api/v1/auth/").WithTags("Auth-Login-Endpoints");
        
        endpoints.MapPost("login",
            async (IAuthService service, DTOLogin DTOLogin) =>
                await service.Authenticate(DTOLogin));
    }
}