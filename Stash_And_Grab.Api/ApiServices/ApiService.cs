using System.Net;
using Stash_And_Grab.Application.ApplicationServices;
using Stash_And_Grab.Application.DtoModels;
using Stash_And_Grab.Application.Logging;

namespace Stash_And_Grab.Api.ApiServices;

internal static class ApiService
{
    internal static async Task<IResult> HandleGetItemRequest(Guid id, IApplicationServices service,
        ILoggerAdaptor<Program> logger)
    {
        try
        {
            var returnValue = await service.GetItem(id);
            return returnValue.IsFailed
                ? Results.NotFound(returnValue.Reasons)
                : Results.Ok(returnValue.Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting result for {Id}", id);
            return Results.Problem("An error occurred while processing your request.",
                statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }

    internal static async Task<IResult> HandleCreateStashRequest(StashCreateDtoModel item, IApplicationServices service,
        ILoggerAdaptor<Program> logger)
    {
        try
        {
            var returnValue = await service.CreateStashItem(item);
            return returnValue.IsFailed
                ? Results.BadRequest(returnValue.Reasons)
                : Results.Created($"/stash/{returnValue.Value.Id}", returnValue.Value.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating {@Item}", item);
            return Results.Problem("An error occurred while processing your request.",
                statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }
}