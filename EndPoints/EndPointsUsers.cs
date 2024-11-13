using HypotheticalRetailStore.DTOs;
using HypotheticalRetailStore.Interfaces;

namespace HypotheticalRetailStore.EndPoints;

public static class EndPointsUsers
{

    public static void ConfigureEndPointsUsers(this WebApplication app)
    {

        var endpoints = app.MapGroup("api/v1/users/").WithTags("Users-Endpoints");

        endpoints.MapPost("add-new",
            async (DTOUser dtouser, IUserService service) =>
                await service.AddNewUser(dtouser)).RequireAuthorization("Admin");

        endpoints.MapPut("update",
                async (Guid id, DTOUser dtouser, IUserService service) =>
                    await service.UpdateUser(dtouser, id))
            .RequireAuthorization("Admin");
        
        endpoints.MapDelete("delete",
                async (Guid id, IUserService service) =>
                    await service.DeleteUser(id))
            .RequireAuthorization("Admin");
        //
        endpoints.MapGet("getall",
            async (IUserService service) =>
                await service.GetAllUsers()).RequireAuthorization("Admin");
    }
}