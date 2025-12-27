using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using BG.Infra.Persistence;
using BG.Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using BG.Domain.Interfaces;
using BG.Domain.Entities;

namespace BG.Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BabylonianDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}