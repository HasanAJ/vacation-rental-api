using System.Threading;
using System.Threading.Tasks;

using VacationRental.Core.Dtos.Booking;
using VacationRental.Core.Dtos.Shared;

namespace VacationRental.Core.Interfaces.Services
{
    public interface IBookingService
    {
        Task<BookingDto> Get(int bookingId, CancellationToken ct);

        Task<ResourceIdDto> Create(BookingBindingDto model, CancellationToken ct);
    }
}