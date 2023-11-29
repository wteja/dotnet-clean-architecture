using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence;

public static class ServiceExtensions
{
    public static void RegisterPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration["UseInMemoryDatabase"] == "True")
        {
            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("ApplicationDb"));
        }
        else
        {
            services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
            b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));
        }
        services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
        services.AddScoped<IAssetTypeRepositoryAsync, AssetTypeRepositoryAsync>();
    }

    public static void AddSeedData(this IServiceProvider provider, IConfiguration configuration)
    {
        if (configuration["UseInMemoryDatabase"] == "True")
        {
            var ctx = provider.GetRequiredService<AppDbContext>();

            if (ctx != null)
            {
                if (!ctx.AssetTypes.Any())
                {
                    ctx.AssetTypes.Add(new Domain.Entities.AssetType
                    {
                        Name = "Asset Type 1",
                    });
                }

                ctx.SaveChangesAsync();
            }
        }
    }
}