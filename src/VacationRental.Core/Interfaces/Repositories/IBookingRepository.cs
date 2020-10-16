using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Core.Interfaces.Repositories.Shared;
using VacationRental.Core.Models.Domain;

namespace VacationRental.Core.Interfaces.Repositories
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task<List<Booking>> Get(int rentalId, DateTime startDate, int nights, int preparationTime, CancellationToken ct);

        Task<List<Booking>> Get(int rentalId, DateTime startDate, CancellationToken ct);
    }
}