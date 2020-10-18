using System;
using System.Collections.Generic;
using VacationRental.Core.Models.Domain;
using VacationRental.Core.Models.Dtos.Rental;

namespace VacationRental.Core.Interfaces.Validators
{
    public interface IUnitValidator
    {
        void Validate(RentalBindingDto model, List<Booking> bookings, DateTime start);
    }
}