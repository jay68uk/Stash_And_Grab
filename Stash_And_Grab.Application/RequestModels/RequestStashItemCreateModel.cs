using Stash_And_Grab.Application.Interfaces;

namespace Stash_And_Grab.Application.RequestModels;

public record RequestStashItemCreateModel(string StashName, string StashData, string StashType);