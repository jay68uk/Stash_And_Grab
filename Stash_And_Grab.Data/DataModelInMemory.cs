using Stash_And_Grab.Application.Interfaces;

namespace Stash_And_Grab.Data.InMemory;

internal record DataModelInMemory : IDataModel
{
    public Guid Id { get; set; }
    public string EntityType { get; set; } = "string";
    public string JsonContent { get; set; } = string.Empty;
    public DateTime LastModified { get; set; }
}