using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Stash_And_Grab.Application.ApplicationServices;
using Stash_And_Grab.Application.Validation;

namespace Stash_And_Grab.Application.Startup;

public static class DependencyInjectionSetup
{
    public static IServiceCollection RegisterLibraryServices(this IServiceCollection services)
    {
        services.AddScoped<IApplicationServices, ApplicationServices.ApplicationServices>();

        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(typeof(MediatREntryPoint).Assembly));

        services.AddScoped<IValidator<StashCreateDtoModel>, StashItemValidation>();

        services.AddTransient(typeof(ILoggerAdaptor<>), typeof(LoggerAdaptor<>));

        return services;
    }
}