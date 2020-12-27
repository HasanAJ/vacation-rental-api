using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Core.Entities.Shared;
using VacationRental.Core.Interfaces.Repositories.Shared;
using VacationRental.Infrastructure.Data.Context;

namespace VacationRental.Infrastructure.Data.Repositories.Shared
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext _db;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<T>> GetAll(CancellationToken ct)
        {
            return await _db.Set<T>().ToListAsync(ct);
        }

        public Task<T> Find(int id, CancellationToken ct)
        {
            return _db.Set<T>().SingleOrDefaultAsync(m => m.Id == id, ct);
        }

        public async Task Add(T entity, CancellationToken ct)
        {
            await _db.Set<T>().AddAsync(entity, ct);
        }

        public void Update(T entity)
        {
            _db.Set<T>().Update(entity);
        }

        public async Task Remove(int id, CancellationToken ct)
        {
            T entity = await _db.Set<T>().SingleOrDefaultAsync(m => m.Id == id, ct);

            _db.Set<T>().Remove(entity);
        }
    }
}