using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Core.Interfaces.Managers;
using VacationRental.Core.Interfaces.UnitOfWork;
using VacationRental.Core.Interfaces.Validators;
using VacationRental.Core.Managers;
using VacationRental.Core.Models.Domain;
using VacationRental.Core.Models.Dtos.Rental;
using Xunit;

namespace VacationRental.UnitTests.Managers
{
    public class UnitManagerTests
    {
        private readonly Mock<IUnitOfWork> _uow;
        private readonly Mock<IUnitValidator> _unitValidator;

        private readonly IUnitManager _unitManager;

        private readonly Rental rental = new Rental()
        {
            Id = 1,
            AllUnits = new List<Unit>()
            {
                new Unit() { Id = 1, RentalId = 1, IsActive = true },
                new Unit() { Id = 2, RentalId = 1, IsActive = true },
                new Unit() { Id = 3, RentalId = 1, IsActive = true }
            },
            PreparationTimeInDays = 1
        };

        private readonly List<Unit> units = new List<Unit>()
            {
                new Unit()
                {
                    Id = 1,
                    RentalId = 1
                },
                new Unit()
                {
                    Id = 2,
                    RentalId = 1
                },
                new Unit()
                {
                    Id = 3,
                    RentalId = 1
                }
            };

        public UnitManagerTests()
        {
            _uow = new Mock<IUnitOfWork>();
            _unitValidator = new Mock<IUnitValidator>();

            _unitManager = new UnitManager(_uow.Object,
                _unitValidator.Object);

            _uow.Setup(x => x.UnitRepository.Get(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(units));
        }

        [Fact]
        public async Task GetUnitId_Success_NoBookings()
        {
            int actual = await _unitManager.GetFreeUnitId(rental.Id, new List<Booking>(), new CancellationToken());

            Assert.Equal(1, actual);
        }

        [Fact]
        public async Task GetUnitId_Success_WithBookings()
        {
            List<Booking> bookings = new List<Booking>()
            {
                new Booking()
                {
                    Id = 1,
                    UnitId = 1,
                    Start = new DateTime(2020, 1, 1),
                    Nights = 2
                },
                new Booking()
                {
                    Id = 2,
                    UnitId = 2,
                    Start = new DateTime(2020, 1, 2),
                    Nights = 2
                },
                new Booking()
                {
                    Id = 3,
                    UnitId = 1,
                    Start = new DateTime(2020, 1, 5),
                    Nights = 1
                }
            };

            int actual = await _unitManager.GetFreeUnitId(rental.Id, bookings, new CancellationToken());

            Assert.Equal(3, actual);
        }

        [Fact]
        public async Task HandleChange_MoreUnits_Success()
        {
            _uow.Setup(x => x.UnitRepository.Add(It.IsAny<Unit>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            RentalBindingDto rentalBindingDto = new RentalBindingDto()
            {
                Units = 5,
                PreparationTimeInDays = 1
            };

            await _unitManager.HandleChange(rental, rentalBindingDto, new CancellationToken());

            _uow.Verify(i => i.UnitRepository.Add(It.IsAny<Unit>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        }
    }
}