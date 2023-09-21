namespace Stash_And_Grab.Application.Queries;

public sealed record GetStashItemByIdQuery(Guid StashItemId) : IRequest<ResponseStashItemModel>;