using System;
using System.Collections.Generic;
using VacationRental.Core.Interfaces.Managers;
using VacationRental.Core.Managers;
using VacationRental.Core.Models.Domain;
using Xunit;

namespace VacationRental.UnitTests.Managers
{
    public class UnitManagerTests
    {
        private readonly IUnitManager _unitManager;

        private readonly Rental rental = new Rental()
        {
            Id = 1,
            Units = 4,
            PreparationTimeInDays = 1
        };

        public UnitManagerTests()
        {
            _unitManager = new UnitManager();
        }

        [Fact]
        public void GetUnitId_Success()
        {
            int actual = _unitManager.GetUnitId(rental, new List<Booking>());

            Assert.Equal(1, actual);
        }

        [Fact]
        public void GetUnitId_Success_2()
        {
            List<Booking> bookings = new List<Booking>()
            {
                new Booking()
                {
                    Id = 1,
                    RentalId = 1,
                    UnitId = 1,
                    Start = new DateTime(2020, 1, 1),
                    Nights = 2
                },
                new Booking()
                {
                    Id = 2,
                    RentalId = 1,
                    UnitId = 2,
                    Start = new DateTime(2020, 1, 2),
                    Nights = 2
                },
                new Booking()
                {
                    Id = 3,
                    RentalId = 1,
                    UnitId = 1,
                    Start = new DateTime(2020, 1, 5),
                    Nights = 1
                }
            };

            int actual = _unitManager.GetUnitId(rental, bookings);

            Assert.Equal(3, actual);
        }
    }
}