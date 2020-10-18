using System.Collections.Generic;
using VacationRental.Core.Common.Constants;
using VacationRental.Core.Exceptions;
using VacationRental.Core.Interfaces.Validators;
using VacationRental.Core.Models.Domain;

namespace VacationRental.Core.Validators
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