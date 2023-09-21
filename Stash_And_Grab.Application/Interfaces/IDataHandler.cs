namespace Stash_And_Grab.Application.Interfaces;

public interface IDataHandler
{
    Task<Guid> InsertStashItem<T>(T item);

    Task<IDataModel?> GetStashItem(Guid id);
}