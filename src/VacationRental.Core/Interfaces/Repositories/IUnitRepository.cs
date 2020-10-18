using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Core.Interfaces.Repositories.Shared;
using VacationRental.Core.Models.Domain;

namespace VacationRental.Core.Interfaces.Repositories
{
    public interface IUnitRepository : IRepository<Unit>
    {
        Task<List<Unit>> Get(int rentalId, CancellationToken ct);
    }
}