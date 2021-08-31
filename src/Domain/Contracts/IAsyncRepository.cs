using System;
using Domain.Entities.Base;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IAsyncRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken);

        Task<IReadOnlyList<T>> ListAllAsync(int perPage = 20, int page = 0, Expression<Func<T, bool>> predicate = null, CancellationToken cancellationToken = default);

        Task<T> AddAsync(T entity, CancellationToken cancellationToken);

        Task UpdateAsync(T entity, CancellationToken cancellationToken);

        Task DeleteAsync(T entity, CancellationToken cancellationToken);
    }
}