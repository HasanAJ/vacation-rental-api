using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Core.Entities;
using VacationRental.Core.Interfaces.Repositories;
using VacationRental.Infrastructure.Data.Context;
using VacationRental.Infrastructure.Data.Repositories.Shared;
using Z.EntityFramework.Plus;

namespace VacationRental.Infrastructure.Data.Repositories
{
    public class RentalRepository : Repository<Rental>, IRentalRepository
    {
        public RentalRepository(ApplicationDbContext _db)
            : base(_db)
        {
        }

        public async Task<Rental> Get(int rentalId, CancellationToken ct)
        {
            return await _db.Set<Rental>()
                .Where(i => i.Id == rentalId)
                .IncludeFilter(c => c.AllUnits.Where(o => o.IsActive == true))
                .FirstOrDefaultAsync(ct);
        }
    }
}