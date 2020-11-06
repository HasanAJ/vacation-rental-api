using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Core.Common.Constants;
using VacationRental.Core.Common.Exceptions;
using VacationRental.Core.Interfaces.Services;
using VacationRental.Core.Interfaces.UnitOfWork;
using VacationRental.Core.Models.Domain;
using VacationRental.Core.Models.Dtos.Calendar;

namespace VacationRental.Core.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly IUnitOfWork _uow;

        public CalendarService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<CalendarDto> Get(int rentalId, DateTime start, int nights, CancellationToken ct)
        {
            Rental rental = await _uow.RentalRepository.Get(rentalId, ct);

            if (rental == null)
            {
                throw new CustomException(ApiCodeConstants.NOT_FOUND, ApiErrorMessageConstants.RENTAL_NOT_FOUND);
            }

            List<Booking> bookings = await _uow.BookingRepository.Get(rentalId, start, nights, rental.PreparationTimeInDays, ct);

            CalendarDto calendar = new CalendarDto
            {
                RentalId = rentalId,
                Dates = new List<CalendarDateDto>()
            };

            for (int i = 0; i < nights; i++)
            {
                CalendarDateDto calendarDate = new CalendarDateDto
                {
                    Date = start.Date.AddDays(i),
                    Bookings = new List<CalendarBookingDto>(),
                    PreparationTimes = new List<PreparationTimeDto>()
                };

                SetupBookings(calendarDate, bookings);

                SetupPreparationTimes(calendarDate, bookings, rental);

                calendar.Dates.Add(calendarDate);
            }

            return calendar;
        }

        private void SetupBookings(CalendarDateDto calendarDate, List<Booking> bookings)
        {
            List<Booking> currentBookings = bookings
                                .Where(i => i.Start <= calendarDate.Date && i.End > calendarDate.Date)
                                .ToList();

            foreach (Booking booking in currentBookings)
            {
                calendarDate.Bookings.Add(new CalendarBookingDto
                {
                    Id = booking.Id,
                    Unit = booking.UnitId
                });
            }
        }

        private void SetupPreparationTimes(CalendarDateDto calendarDate, List<Booking> bookings, Rental rental)
        {
            List<Booking> preparingUnits = bookings
                    .Where(i => i.End <= calendarDate.Date && i.End.AddDays(rental.PreparationTimeInDays) > calendarDate.Date)
                    .ToList();

            foreach (Booking booking in preparingUnits)
            {
                calendarDate.PreparationTimes.Add(new PreparationTimeDto() { Unit = booking.UnitId });
            }
        }
    }
}