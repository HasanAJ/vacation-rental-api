using System.Threading;
using System.Threading.Tasks;
using VacationRental.Core.Entities;
using VacationRental.Core.Interfaces.Repositories.Shared;

namespace VacationRental.Core.Interfaces.Repositories
{
    public interface IRentalRepository : IRepository<Rental>
    {
        Task<Rental> Get(int rentalId, CancellationToken ct);
    }
}