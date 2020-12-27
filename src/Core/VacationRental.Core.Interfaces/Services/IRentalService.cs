using System.Threading;
using System.Threading.Tasks;
using VacationRental.Core.Dtos.Rental;
using VacationRental.Core.Dtos.Shared;

namespace VacationRental.Core.Interfaces.Services
{
    public interface IRentalService
    {
        Task<RentalDto> Get(int rentalId, CancellationToken ct);

        Task<ResourceIdDto> Create(RentalBindingDto model, CancellationToken ct);

        Task Update(int rentalId, RentalBindingDto model, CancellationToken ct);
    }
}