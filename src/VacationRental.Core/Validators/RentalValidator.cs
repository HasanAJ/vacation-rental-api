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
    public class RentalValidator : IRentalValidator
    {
        private readonly IUnitOfWork _uow;

        public RentalValidator(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task Validate(Rental rental, RentalBindingDto model, CancellationToken ct)
        {
            if (rental == null)
            {
                throw new CustomException(ApiCodeConstants.NOT_FOUND, ApiErrorMessageConstants.RENTAL_NOT_FOUND, HttpStatusCode.NotFound);
            }

            if (model.Units < rental.Units || model.PreparationTimeInDays > rental.PreparationTimeInDays)
            {
                DateTime start = DateTime.UtcNow;

                List<Booking> bookings = await _uow.BookingRepository.Get(rental.Id, start, ct);

                DateTime latestDateBooking = bookings
                                                .OrderByDescending(i => i.End)
                                                .Select(i => i.End)
                                                .FirstOrDefault();

                if (bookings != null)
                {
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
}