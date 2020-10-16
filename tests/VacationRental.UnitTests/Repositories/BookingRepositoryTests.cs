using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Core.Interfaces.Repositories;
using VacationRental.Core.Models.Domain;
using VacationRental.Infrastructure.Context;
using VacationRental.Infrastructure.Repositories;
using Xunit;

namespace VacationRental.UnitTests.Repositories
{
    public class BookingRepositoryTests
    {
        private readonly List<Booking> bookings = new List<Booking>()
        {
            new Booking()
            {
                Id = 1,
                RentalId = 1,
                Start = new DateTime(2020, 1, 1),
                Nights = 2
            },
            new Booking()
            {
                Id = 2,
                RentalId = 1,
                Start = new DateTime(2020, 1, 2),
                Nights = 2
            },
            new Booking()
            {
                Id = 3,
                RentalId = 1,
                Start = new DateTime(2020, 1, 4),
                Nights = 2
            }
        };

        private readonly ApplicationDbContext _dbContext;
        private readonly IBookingRepository _bookingRepository;

        public BookingRepositoryTests()
        {
            _dbContext = new MockDbContext().GetDbContext();
            _bookingRepository = new BookingRepository(_dbContext);

            if (!_dbContext.Booking.Any())
            {
                _dbContext.Booking.AddRange(bookings);
                _dbContext.SaveChanges();
            }
        }

        [Fact]
        public async Task Get_Success()
        {
            int rentalId = 1;
            DateTime startDate = new DateTime(2020, 1, 2);
            int preparationTime = 2;
            int nights = 2;

            List<Booking> actual = await _bookingRepository.Get(rentalId, startDate, nights, preparationTime, new CancellationToken());

            List<Booking> expected = bookings
                .Where(i => (i.RentalId == rentalId)
                            && ((i.Start <= startDate.Date && i.End.AddDays(preparationTime) > startDate.Date)
                            || (i.Start < startDate.AddDays(nights).AddDays(preparationTime) && i.End.AddDays(preparationTime) >= startDate.AddDays(nights).AddDays(preparationTime))
                            || (i.Start > startDate && i.End.AddDays(preparationTime) < startDate.AddDays(nights).AddDays(preparationTime))))
               .ToList();

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task Get_Success_2()
        {
            int rentalId = 1;
            DateTime startDate = new DateTime(2020, 1, 2);

            List<Booking> actual = await _bookingRepository.Get(rentalId, startDate, new CancellationToken());

            List<Booking> expected = bookings
                .Where(i => (i.RentalId == rentalId
                        && (i.Start.AddDays(i.Nights) >= startDate.Date)))
               .ToList();

            actual.Should().BeEquivalentTo(expected);
        }
    }
}