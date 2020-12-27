using System.Collections.Generic;
using VacationRental.Core.Entities;

namespace VacationRental.Core.Interfaces.Validators
{
    public interface IBookingValidator
    {
        void Validate(Rental rental, List<Booking> currentBookings);
    }
}