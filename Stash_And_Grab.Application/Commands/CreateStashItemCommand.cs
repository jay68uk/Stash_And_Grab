namespace Stash_And_Grab.Application.Commands;

public sealed record CreateStashItemCommand
    (RequestStashItemCreateModel ItemData) : IRequest<ResponseStashItemStatusModel>;