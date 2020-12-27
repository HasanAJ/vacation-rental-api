using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Core.Dtos.Rental;
using VacationRental.Core.Entities;

namespace VacationRental.Core.Interfaces.Managers
{
    public interface IUnitManager
    {
        Task<int> GetFreeUnitId(int rentalId, List<Booking> occupiedUnits, CancellationToken ct);

        Task HandleChange(Rental rental, RentalBindingDto model, CancellationToken ct);
    }
}