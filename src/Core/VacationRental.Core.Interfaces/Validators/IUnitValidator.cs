using System;
using System.Collections.Generic;
using VacationRental.Core.Dtos.Rental;
using VacationRental.Core.Entities;

namespace VacationRental.Core.Interfaces.Validators
{
    public interface IUnitValidator
    {
        void Validate(RentalBindingDto model, List<Booking> bookings, DateTime start);
    }
}