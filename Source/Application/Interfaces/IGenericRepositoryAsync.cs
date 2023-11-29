namespace Application.Interfaces;

public interface IGenericRepositoryAsync<T>
{
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<T?> GetByIdForRefAsync(int id, CancellationToken cancellationToken);

    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken);

    Task<IReadOnlyList<T>> GetPagedResponseAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);

    Task<T> AddAsync(T entity, CancellationToken cancellationToken);

    Task UpdateAsync(T entity, CancellationToken cancellationToken);

    Task DeleteAsync(T entity, CancellationToken cancellationToken);
}