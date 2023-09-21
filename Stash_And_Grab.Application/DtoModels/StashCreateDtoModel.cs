using Stash_And_Grab.Application.Interfaces;

namespace Stash_And_Grab.Application.DtoModels;

public sealed record StashCreateDtoModel(string StashName, string StashData, string StashType);