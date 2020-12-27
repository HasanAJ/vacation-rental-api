using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Core.Entities;
using VacationRental.Core.Interfaces.Repositories;
using VacationRental.Infrastructure.Data.Context;
using VacationRental.Infrastructure.Data.Repositories;
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

            if (!_dbContext.Rentals.Any())
            {
                _dbContext.Rentals.Add(rental);
            }
            if (!_dbContext.Units.Any())
            {
                _dbContext.Units.AddRange(units);
            }
            if (!_dbContext.Bookings.Any())
            {
                _dbContext.Bookings.AddRange(bookings);
            }

            _dbContext.SaveChanges();
        }

        [Fact]
        public async Task Get_Success_RentalWithPreparationTime()
        {
            DateTime startDate = new DateTime(2020, 1, 2);
            int preparationTime = 2;
            int nights = 2;

            List<Booking> actual = await _bookingRepository.Get(rental.Id, startDate, nights, preparationTime, new CancellationToken());

            List<Booking> expected = new List<Booking>()
            {
                new Booking()
                {
                    Id = 1,
                    Start = new DateTime(2020, 1, 1),
                    Nights = 2,
                    UnitId = 3
                },
                new Booking()
                {
                    Id = 2,
                    Start = new DateTime(2020, 1, 2),
                    Nights = 2,
                    UnitId = 3
                },
                new Booking()
                {
                    Id = 3,
                    Start = new DateTime(2020, 1, 4),
                    Nights = 2,
                    UnitId = 3
                }
            };

            Assert.Equal(expected.Count, actual.Count);

            for (int i = 0; i < actual.Count; i++)
            {
                Assert.Equal(expected[0].Id, actual[0].Id);
                Assert.Equal(expected[0].Start, actual[0].Start);
                Assert.Equal(expected[0].Nights, actual[0].Nights);
                Assert.Equal(expected[0].UnitId, actual[0].Unit.Id);
            }
        }

        [Fact]
        public async Task Get_Success_Rental()
        {
            DateTime startDate = new DateTime(2020, 1, 4);

            List<Booking> actual = await _bookingRepository.Get(rental.Id, startDate, new CancellationToken());

            List<Booking> expected = new List<Booking>()
            {
                new Booking()
                {
                    Id = 2,
                    Start = new DateTime(2020, 1, 2),
                    Nights = 2,
                    UnitId = 3
                },
                new Booking()
                {
                    Id = 3,
                    Start = new DateTime(2020, 1, 4),
                    Nights = 2,
                    UnitId = 3
                }
            };

            Assert.Equal(expected.Count, actual.Count);

            for (int i = 0; i < actual.Count; i++)
            {
                Assert.Equal(expected[0].Id, actual[0].Id);
                Assert.Equal(expected[0].Start, actual[0].Start);
                Assert.Equal(expected[0].Nights, actual[0].Nights);
                Assert.Equal(expected[0].UnitId, actual[0].Unit.Id);
            }
        }

        [Fact]
        public async Task Get_Success_BookingId()
        {
            Booking actual = await _bookingRepository.Get(1, new CancellationToken());

            Booking expected = new Booking()
            {
                Id = 1,
                Start = new DateTime(2020, 1, 1),
                Nights = 2,
                UnitId = 3
            };

            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Start, actual.Start);
            Assert.Equal(expected.Nights, actual.Nights);
            Assert.Equal(expected.UnitId, actual.Unit.Id);
        }
    }
}