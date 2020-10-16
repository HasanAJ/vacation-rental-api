using System;
using System.Collections.Generic;
using VacationRental.Core.Common.Constants;
using VacationRental.Core.Exceptions;
using VacationRental.Core.Interfaces.Validators;
using VacationRental.Core.Models.Domain;
using VacationRental.Core.Models.Dtos.Booking;
using VacationRental.Core.Validators;
using Xunit;

namespace VacationRental.UnitTests.Validators
{
    public class BookingValidatorTests
    {
        private readonly IBookingValidator _bookingValidator;

        private readonly Rental rental = new Rental()
        {
            Id = 1,
            Units = 1,
            PreparationTimeInDays = 1
        };

        private readonly BookingBindingDto bookingBindingDto = new BookingBindingDto()
        {
            RentalId = 1,
            Start = DateTime.UtcNow,
            Nights = 2
        };

        public BookingValidatorTests()
        {
            _bookingValidator = new BookingValidator();
        }

        [Fact]
        public void Validate_Success()
        {
            _bookingValidator.Validate(rental, new List<Booking>());
        }

        [Fact]
        public void Validate_Fail_NotAvailable()
        {
            List<Booking> bookings = new List<Booking>()
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
                    Start = new DateTime(2020, 1, 3),
                    Nights = 2
                }
            };

            CustomException exception = Assert.Throws<CustomException>(() => _bookingValidator.Validate(rental, bookings));

            Assert.Equal(ApiCodeConstants.NOT_AVAILABLE, exception.Code);
        }
    }
}