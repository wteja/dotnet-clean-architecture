using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IAssetTypeRepositoryAsync : IGenericRepositoryAsync<AssetType>
{
    Task<bool> IsUniqueName(string name, int? skipId, CancellationToken cancellationToken);
}