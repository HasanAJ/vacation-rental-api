using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Core.Entities.Shared;

namespace VacationRental.Core.Interfaces.Repositories.Shared
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<List<T>> GetAll(CancellationToken ct);

        Task<T> Find(int Id, CancellationToken ct);

        Task Add(T entity, CancellationToken ct);

        void Update(T entity);

        Task Remove(int Id, CancellationToken ct);
    }
}