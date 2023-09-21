using Stash_And_Grab.Application.Interfaces;

namespace Stash_And_Grab.Data.InMemory;

public sealed class DataHandler : IDataHandler
{
    public Task<Guid> InsertStashItem<T>(T item)
    {
        var stash = item as DataModelInMemory;
        stash!.Id = Guid.NewGuid();
        InMemoryData.Records.Add(stash);
        return Task.FromResult(stash.Id);
    }

    public Task<IDataModel?> GetStashItem(Guid id)
    {
        var result = InMemoryData.Records.FirstOrDefault(x => x.Id == id);

        return Task.FromResult(result);
    }
}