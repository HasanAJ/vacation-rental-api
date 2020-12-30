using System.Collections.Generic;
using VacationRental.Common.Constants;
using VacationRental.Common.Exceptions;
using VacationRental.Core.Entities;
using VacationRental.Core.Interfaces.Validators;

namespace VacationRental.Core.Application.Validators
{
    public class BookingValidator : IBookingValidator
    {
        public BookingValidator()
        {
        }

        public void Validate(Rental rental, List<Booking> currentBookings)
        {
            if (currentBookings.Count >= rental.AllUnits.Count)
            {
                throw new CustomException(ApiCodeConstants.NOT_AVAILABLE, ApiErrorMessageConstants.BOOKING_NOT_AVAILABLE);
            }
        }
    }
}