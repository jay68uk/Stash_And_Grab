using Microsoft.AspNetCore.Mvc;
using Stash_And_Grab.Api.ApiServices;
using Stash_And_Grab.Application.ApplicationServices;
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
                    await ApiService.HandleGetItemRequest(id, service, logger))
            .WithName(RoutingConstants.GetStashItem) //e.g. ItemById
            .WithTags(RoutingConstants.StashAndGrab); //e.g. ItemApi

        app.MapPost("/stash",
                [SwaggerOperation(Summary = "Create a stash entry",
                    Description = "Creates a stash entry and returns the Id of the stash for later use.")]
                [SwaggerResponse(201, "Created")]
                [SwaggerResponse(400, "Bad Request")]
                [SwaggerResponse(404, "Not Found")]
                async ([FromBody] StashCreateDtoModel item, [FromServices] IConfiguration configuration,
                        [FromServices] ILoggerAdaptor<Program> logger, [FromServices] IApplicationServices service) =>
                    await ApiService.HandleCreateStashRequest(item, service, logger))
            .WithName(RoutingConstants.CreateStashItem) //e.g. CreateStashItem
            .WithTags(RoutingConstants.StashAndGrab); //e.g. ItemApi
    }
}