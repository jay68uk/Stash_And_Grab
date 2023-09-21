using Stash_And_Grab.Application.Interfaces;

namespace Stash_And_Grab.Application.DtoModels;

internal record DataModel : IDataModel
{
    public Guid Id { get; set; }
    public string EntityType { get; set; } = string.Empty;
    public string JsonContent { get; set; } = string.Empty;
    public DateTime LastModified { get; set; }
}