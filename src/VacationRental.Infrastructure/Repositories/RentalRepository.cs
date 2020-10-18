using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Core.Interfaces.Repositories;
using VacationRental.Core.Models.Domain;
using VacationRental.Infrastructure.Context;
using VacationRental.Infrastructure.Repositories.Shared;
using Z.EntityFramework.Plus;

namespace VacationRental.Infrastructure.Repositories
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