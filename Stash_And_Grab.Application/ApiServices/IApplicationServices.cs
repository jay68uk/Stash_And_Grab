using FluentResults;

namespace Stash_And_Grab.Application.ApiServices;

public interface IApplicationServices
{
    Task<Result<ResponseStashItemStatusModel>> CreateStashItem(StashCreateDtoModel item);

    Task<Result<ResponseStashItemModel>> GetItem(Guid id);
}