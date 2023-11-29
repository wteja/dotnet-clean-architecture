using Application.Interfaces;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Domain;

public class GenericRepositoryAsync<T> : IGenericRepositoryAsync<T> where T : class
{
    protected readonly AppDbContext _dbContext;

    public GenericRepositoryAsync(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Set<T>().ToListAsync(cancellationToken);
    }

    public virtual async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var data = await _dbContext.Set<T>().FindAsync(id, cancellationToken);
        if (data != null)
        {
            _dbContext.Entry(data).State = EntityState.Detached;
        }
        return data;
    }

    public virtual async Task<T?> GetByIdForRefAsync(int id, CancellationToken cancellationToken)
    {
        var data = await _dbContext.Set<T>().FindAsync(id, cancellationToken);
        return data;
    }

    public virtual async Task<IReadOnlyList<T>> GetPagedResponseAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<T>()
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .AsNoTracking()
        .ToListAsync(cancellationToken);
    }

    public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
    {
        await _dbContext.Set<T>().AddAsync(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task DeleteAsync(T entity, CancellationToken cancellationToken)
    {
        _dbContext.Set<T>().Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}