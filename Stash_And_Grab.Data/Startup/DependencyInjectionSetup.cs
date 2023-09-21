using Microsoft.Extensions.DependencyInjection;
using Stash_And_Grab.Application.Interfaces;

namespace Stash_And_Grab.Data.InMemory.Startup;

public static class DependencyInjectionSetup
{
    public static IServiceCollection RegisterInMemoryDataServices(this IServiceCollection services)
    {
        services.AddSingleton<IDataHandler, DataHandler>();
        return services;
    }
}