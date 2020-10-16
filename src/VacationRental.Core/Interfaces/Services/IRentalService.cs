using System.Threading;
using System.Threading.Tasks;
using VacationRental.Core.Models.Dtos.Rental;
using VacationRental.Core.Models.Dtos.Shared;

namespace VacationRental.Core.Interfaces.Services
{
    public interface IRentalService
    {
        Task<RentalDto> Get(int rentalId, CancellationToken ct);

        Task<ResourceIdDto> Create(RentalBindingDto model, CancellationToken ct);

        Task Update(int rentalId, RentalBindingDto model, CancellationToken ct);
    }
}