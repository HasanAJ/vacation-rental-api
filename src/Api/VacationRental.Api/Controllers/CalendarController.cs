using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Api.Controllers.Shared;
using VacationRental.Core.Dtos.Calendar;
using VacationRental.Core.Interfaces.Services;

namespace VacationRental.Api.Controllers
{
    public class CalendarController : BaseController
    {
        private readonly ICalendarService _calendarService;

        public CalendarController(ICalendarService calendarService)
        {
            _calendarService = calendarService;
        }

        [HttpGet("")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(CalendarDto))]
        public async Task<IActionResult> Get(int rentalId, DateTime start, int nights, CancellationToken ct)
        {
            CalendarDto calendar = await _calendarService.Get(rentalId, start, nights, ct);

            return Ok(calendar);
        }
    }
}