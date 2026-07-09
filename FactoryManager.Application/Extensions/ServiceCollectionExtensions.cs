using FactoryManager.Application.Services;
using FactoryManager.Application.Services.Interfaces;
using FactoryManager.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace FactoryManager.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IMachineService, MachineService>();
        services.AddValidatorsFromAssemblyContaining<CreateMachineValidator>();
        return services;
    }
}