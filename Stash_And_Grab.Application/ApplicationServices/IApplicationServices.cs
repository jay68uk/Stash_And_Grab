using FluentResults;

namespace Stash_And_Grab.Application.ApplicationServices;

public interface IApplicationServices
{
    Task<Result<ResponseStashItemStatusModel>> CreateStashItem(StashCreateDtoModel item);

    Task<Result<ResponseStashItemModel>> GetItem(Guid id);
}