using Microsoft.AspNetCore.Mvc;
using Stash_And_Grab.Application.ApiServices;
using Stash_And_Grab.Application.DtoModels;
using Stash_And_Grab.Application.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace Stash_And_Grab.Api.Endpoints;

public static class MapEndpoints
{
    public static void MapServiceEndpoints(WebApplication app)
    {
        app.MapGet("/stash/{id}",
                [SwaggerOperation(Summary = "Get a stashed item",
                    Description = "Gets a stashed item based on Id")]
                [SwaggerResponse(200, "Success")]
                [SwaggerResponse(400, "Bad Request")]
                [SwaggerResponse(404, "Not Found")]
                async ([FromRoute] Guid id, [FromServices] IConfiguration configuration,
                    [FromServices] ILoggerAdaptor<Program> logger, [FromServices] IApplicationServices service) =>
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
                        throw;
                    }
                })
            .WithName("GET_STASH_ITEM") //e.g. ItemById
            .WithTags("STASH_AND_GRAB"); //e.g. ItemApi

        app.MapPost("/stash",
                [SwaggerOperation(Summary = "Create a stash entry",
                    Description = "Creates a stash entry and returns the Id of the stash for later use.")]
                [SwaggerResponse(201, "Created")]
                [SwaggerResponse(400, "Bad Request")]
                [SwaggerResponse(404, "Not Found")]
                async ([FromBody] StashCreateDtoModel item, [FromServices] IConfiguration configuration,
                    [FromServices] ILoggerAdaptor<Program> logger, [FromServices] IApplicationServices service) =>
                {
                    try
                    {
                        var returnValue = await service.CreateStashItem(item);

                        return returnValue.IsFailed
                            ? Results.BadRequest(returnValue.Reasons)
                            : Results.Created($"/stats/{returnValue.Value.Id}", returnValue.Value.Id);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error creating {@Item}", item);
                        throw;
                    }
                })
            .WithName("CREATE_STASH_ITEM") //e.g. CreateStashItem
            .WithTags("STASH_AND_GRAB"); //e.g. ItemApi
    }
}