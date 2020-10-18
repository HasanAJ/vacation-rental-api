using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Core.Common.Constants;
using VacationRental.Core.Exceptions;
using VacationRental.Core.Interfaces.UnitOfWork;
using VacationRental.Core.Interfaces.Validators;
using VacationRental.Core.Models.Domain;
using VacationRental.Core.Models.Dtos.Rental;

namespace VacationRental.Core.Validators
{
    public class UnitValidator : IUnitValidator
    {
        public void Validate(RentalBindingDto model, List<Booking> bookings, DateTime start)
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