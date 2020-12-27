using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Core.Entities;
using VacationRental.Core.Interfaces.Repositories;
using VacationRental.Infrastructure.Data.Context;
using VacationRental.Infrastructure.Data.Repositories.Shared;

namespace VacationRental.Infrastructure.Data.Repositories
{
    public class UnitRepository : Repository<Unit>, IUnitRepository
    {
        public UnitRepository(ApplicationDbContext _db)
            : base(_db)
        {
        }

        public async Task<List<Unit>> Get(int rentalId, CancellationToken ct)
        {
            return await _db.Set<Unit>()
                .Where(i => i.RentalId == rentalId && i.IsActive == true)
                .ToListAsync(ct);
        }
    }
}