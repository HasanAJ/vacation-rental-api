using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Common.Constants;
using VacationRental.Common.Exceptions;
using VacationRental.Core.Dtos.Rental;
using VacationRental.Core.Entities;
using VacationRental.Core.Interfaces.Validators;

namespace VacationRental.Core.Services.Validators
{
    public class UnitValidator : IUnitValidator
    {
        public void Validate(RentalBindingDto model, List<Booking> bookings, DateTime start)
        {
            if (bookings != null && bookings.Any())
            {
                DateTime latestDateBooking = bookings
                                              .OrderByDescending(i => i.End)
                                              .Select(i => i.End)
                                              .FirstOrDefault();

                int nights = (latestDateBooking - start).Days;

                for (int i = 0; i < nights; i++)
                {
                    DateTime calendarDate = start.Date.AddDays(i);

                    int currentBookings = bookings
                        .Count(i => i.Start <= calendarDate
                                && i.End > calendarDate);

                    int preparingUnits = bookings
                        .Count(i => i.End <= calendarDate
                                && i.End.AddDays(model.PreparationTimeInDays) > calendarDate);

                    if (currentBookings + preparingUnits > model.Units)
                    {
                        throw new CustomException(ApiCodeConstants.INVALID_UPDATE, ApiErrorMessageConstants.INVALID_UPDATE);
                    }
                }
            }
        }
    }
}