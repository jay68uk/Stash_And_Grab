using Stash_And_Grab.Application.Interfaces;

namespace Stash_And_Grab.Application.ApplicationServices;

internal static class MapperService
{
    internal static RequestStashItemCreateModel ToRequestStashItemCreateModel(this StashCreateDtoModel source)
    {
        return new RequestStashItemCreateModel(source.StashName, source.StashData, source.StashType);
    }

    internal static DataModel ToDataModel(this RequestStashItemCreateModel source)
    {
        return new DataModel
        {
            Id = Guid.Empty,
            EntityType = source.StashType,
            JsonContent = source.StashData,
            LastModified = DateTime.UtcNow
        };
    }

    internal static DataModel ToDataModel(this IDataModel model)
    {
        return new DataModel
        {
            Id = model.Id,
            EntityType = model.EntityType,
            JsonContent = model.JsonContent,
            LastModified = model.LastModified
        };
    }

    internal static ResponseStashItemModel ToResponseStashItemModel(this IDataModel source)
    {
        return new ResponseStashItemModel(
            StashConverterService.DeserialiseReturn(source.JsonContent, source.EntityType));
    }
}