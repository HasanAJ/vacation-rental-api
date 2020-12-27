using System.Threading;
using System.Threading.Tasks;
using VacationRental.Core.Interfaces.Repositories;

namespace VacationRental.Core.Interfaces.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRentalRepository RentalRepository { get; }

        IBookingRepository BookingRepository { get; }
        IUnitRepository UnitRepository { get; }

        Task<int> Commit(CancellationToken ct);

        Task Dispose();
    }
}