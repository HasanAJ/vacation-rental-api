using System;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Core.Models.Dtos.Calendar;

namespace VacationRental.Core.Interfaces.Services
{
    public interface ICalendarService
    {
        Task<CalendarDto> Get(int rentalId, DateTime start, int nights, CancellationToken ct);
    }
}