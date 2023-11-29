using Application.Interfaces.Repositories;
using Domain;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AssetTypeRepositoryAsync : GenericRepositoryAsync<AssetType>, IAssetTypeRepositoryAsync
{
    public AssetTypeRepositoryAsync(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> IsUniqueName(string name, int? skipId, CancellationToken cancellationToken)
    {
        var found = false;
        if (skipId != null)
        {
            found = await _dbContext.Set<AssetType>().AnyAsync(it => it.Name != null && it.Name.Contains(name) && it.Id != skipId, cancellationToken);
        }
        else
        {
            found = await _dbContext.Set<AssetType>().AnyAsync(it => it.Name != null && it.Name.Contains(name), cancellationToken);
        }
        return !found;
    }
}