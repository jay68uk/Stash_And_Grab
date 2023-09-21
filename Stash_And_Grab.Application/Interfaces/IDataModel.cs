namespace Stash_And_Grab.Application.Interfaces;

public interface IDataModel
{
    Guid Id { get; set; }
    string EntityType { get; set; }
    string JsonContent { get; set; }
    DateTime LastModified { get; set; }
}