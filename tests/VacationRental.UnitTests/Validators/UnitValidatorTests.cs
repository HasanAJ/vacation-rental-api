using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Core.Common.Constants;
using VacationRental.Core.Exceptions;
using VacationRental.Core.Interfaces.UnitOfWork;
using VacationRental.Core.Interfaces.Validators;
using VacationRental.Core.Models.Domain;
using VacationRental.Core.Models.Dtos.Rental;
using VacationRental.Core.Validators;
using Xunit;

namespace VacationRental.UnitTests.Validators
{
    public class UnitValidatorTests
    {
        private readonly Mock<IUnitOfWork> _uow;

        private readonly IUnitValidator _unitValidator;

        private readonly Rental rental = new Rental()
        {
            Id = 1,
            AllUnits = new List<Unit>() { new Unit() { Id = 1, RentalId = 1, IsActive = true }, new Unit() { Id = 2, RentalId = 1, IsActive = true } },
            PreparationTimeInDays = 1
        };

        private readonly List<Booking> bookings = new List<Booking>()
        {
            new Booking()
            {
                Id = 1,
                UnitId = 1,
                Start = DateTime.UtcNow.Date,
                Nights = 1
            },
            new Booking()
            {
                Id = 2,
                UnitId = 2,
                Start = DateTime.UtcNow.AddDays(1).Date,
                Nights = 1
            },
            new Booking()
            {
                Id = 3,
                UnitId = 1,
                Start =  DateTime.UtcNow.AddDays(4).Date,
                Nights = 1
            }
        };

        public UnitValidatorTests()
        {
            _uow = new Mock<IUnitOfWork>();

            _unitValidator = new UnitValidator();
        }

        [Theory]
        [InlineData(3, 1)]
        [InlineData(4, 0)]
        [InlineData(5, 1)]
        public void Validate_Success(int units, int preparationTime)
        {
            RentalBindingDto rentalBindingDto = new RentalBindingDto()
            {
                Units = units,
                PreparationTimeInDays = preparationTime
            };

            _unitValidator.Validate(rentalBindingDto, bookings, DateTime.UtcNow.Date);
        }

        [Theory]
        [InlineData(1, 3)]
        [InlineData(1, 4)]
        [InlineData(0, 5)]
        public void Validate_Fail_InvalidUpdate(int units, int preparationTime)
        {
            _uow.Setup(x => x.BookingRepository.Get(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(bookings));

            RentalBindingDto rentalBindingDto = new RentalBindingDto()
            {
                Units = units,
                PreparationTimeInDays = preparationTime
            };

            CustomException exception = Assert.Throws<CustomException>(() => _unitValidator.Validate(rentalBindingDto, bookings, DateTime.UtcNow.Date));

            Assert.Equal(ApiCodeConstants.INVALID_UPDATE, exception.Code);
        }
    }
}