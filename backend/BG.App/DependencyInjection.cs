using BG.App.Interfaces;
using BG.App.Services;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using BG.App.Validators.Soldiers;

namespace BG.App;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ISoldierService, SoldierService>();
        services.AddScoped<IWeaponService, WeaponService>();
        services.AddScoped<IAmmoService, AmmoService>();
        services.AddScoped<ILogService, LogService>();

        services.AddValidatorsFromAssemblyContaining<CreateSoldierValidator>();

        return services;
    }
}