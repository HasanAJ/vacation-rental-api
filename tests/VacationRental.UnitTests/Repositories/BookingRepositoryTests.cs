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
        private static readonly Rental rental = new Rental
        {
            Id = 2,
            PreparationTimeInDays = 1
        };

        private static readonly List<Unit> units = new List<Unit>()
        {
            new Unit()
            {
                Id = 3,
                Rental = rental,
                IsActive = true
            },
            new Unit()
            {
                Id = 4,
                Rental = rental,
                IsActive = true
            }
        };

        private readonly List<Booking> bookings = new List<Booking>()
        {
            new Booking()
            {
                Id = 1,
                Start = new DateTime(2020, 1, 1),
                Nights = 2,
                Unit = units[0]
            },
            new Booking()
            {
                Id = 2,
                Start = new DateTime(2020, 1, 2),
                Nights = 2,
                Unit = units[0]
            },
            new Booking()
            {
                Id = 3,
                Start = new DateTime(2020, 1, 4),
                Nights = 2,
                Unit = units[0]
            }
        };

        private readonly ApplicationDbContext _dbContext;
        private readonly IBookingRepository _bookingRepository;

        public BookingRepositoryTests()
        {
            _dbContext = new MockDbContext().GetDbContext();
            _bookingRepository = new BookingRepository(_dbContext);

            if (!_dbContext.Rental.Any())
            {
                _dbContext.Rental.Add(rental);
            }
            if (!_dbContext.Unit.Any())
            {
                _dbContext.Unit.AddRange(units);
            }
            if (!_dbContext.Booking.Any())
            {
                _dbContext.Booking.AddRange(bookings);
            }

            _dbContext.SaveChanges();
        }

        [Fact]
        public async Task Get_Success()
        {
            DateTime startDate = new DateTime(2020, 1, 2);
            int preparationTime = 2;
            int nights = 2;

            List<Booking> actual = await _bookingRepository.Get(rental.Id, startDate, nights, preparationTime, new CancellationToken());

            List<Booking> expected = bookings
                .Where(i => (i.Unit.RentalId == rental.Id)
                            && ((i.Start <= startDate.Date && i.End.AddDays(preparationTime) > startDate.Date)
                            || (i.Start < startDate.AddDays(nights).AddDays(preparationTime) && i.End.AddDays(preparationTime) >= startDate.AddDays(nights).AddDays(preparationTime))
                            || (i.Start > startDate && i.End.AddDays(preparationTime) < startDate.AddDays(nights).AddDays(preparationTime))))
               .ToList();

            Assert.Equal(expected.Count, actual.Count);

            for (int i = 0; i < actual.Count; i++)
            {
                Assert.Equal(expected[0].Id, actual[0].Id);
            }
        }

        [Fact]
        public async Task Get_Success_2()
        {
            DateTime startDate = new DateTime(2020, 1, 2);

            List<Booking> actual = await _bookingRepository.Get(rental.Id, startDate, new CancellationToken());

            List<Booking> expected = bookings
                .Where(i => i.Unit.RentalId == rental.Id
                        && (i.Start.AddDays(i.Nights) >= startDate.Date))
               .ToList();

            Assert.Equal(expected.Count, actual.Count);

            for (int i = 0; i < actual.Count; i++)
            {
                Assert.Equal(expected[0].Id, actual[0].Id);
            }
        }

        [Fact]
        public async Task Get_Success_3()
        {
            Booking actual = await _bookingRepository.Get(bookings[0].Id, new CancellationToken());

            Booking expected = bookings
                .Where(i => i.Id == bookings[0].Id)
               .FirstOrDefault();

            Assert.Equal(expected.Id, actual.Id);
        }
    }
}