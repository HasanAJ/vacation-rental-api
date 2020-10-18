using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Core.Interfaces.Repositories;
using VacationRental.Core.Models.Domain;
using VacationRental.Infrastructure.Context;
using VacationRental.Infrastructure.Repositories.Shared;

namespace VacationRental.Infrastructure.Repositories
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        public BookingRepository(ApplicationDbContext _db)
            : base(_db)
        {
        }

        public async Task<Booking> Get(int bookingId, CancellationToken ct)
        {
            return await _db.Set<Booking>()
                .Where(i => i.Id == bookingId)
                .Include(i => i.Unit)
                .SingleOrDefaultAsync(ct);
        }

        public async Task<List<Booking>> Get(int rentalId, DateTime startDate, int nights, int preparationTime, CancellationToken ct)
        {
            var allbookings = await _db.Set<Booking>().ToListAsync(ct);

            return await _db.Set<Booking>()
                .Where(i => (i.Unit.RentalId == rentalId && i.Unit.IsActive == true)
                        && ((i.Start <= startDate.Date && i.Start.AddDays(i.Nights).AddDays(preparationTime) > startDate.Date)
                        || (i.Start < startDate.AddDays(nights).AddDays(preparationTime) && i.Start.AddDays(i.Nights).AddDays(preparationTime) >= startDate.AddDays(nights).AddDays(preparationTime))
                        || (i.Start > startDate && i.Start.AddDays(i.Nights).AddDays(preparationTime) < startDate.AddDays(nights).AddDays(preparationTime))))
                .Include(i => i.Unit)
                .ToListAsync(ct);
        }

        public async Task<List<Booking>> Get(int rentalId, DateTime startDate, CancellationToken ct)
        {
            return await _db.Set<Booking>()
                .Where(i => i.Unit.RentalId == rentalId
                        && i.Unit.IsActive == true
                        && (i.Start.AddDays(i.Nights) >= startDate.Date))
                .Include(i => i.Unit)
                .ToListAsync(ct);
        }
    }
}