using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Api.Controllers.Shared;
using VacationRental.Core.Dtos.Booking;
using VacationRental.Core.Dtos.Shared;
using VacationRental.Core.Interfaces.Services;

namespace VacationRental.Api.Controllers
{
    public class BookingsController : BaseController
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("{bookingId:int}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(BookingDto))]
        public async Task<IActionResult> Get(int bookingId, CancellationToken ct)
        {
            BookingDto booking = await _bookingService.Get(bookingId, ct);

            return Ok(booking);
        }

        [HttpPost("")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(ResourceIdDto))]
        public async Task<IActionResult> Post(BookingBindingDto model, CancellationToken ct)
        {
            ResourceIdDto resourceId = await _bookingService.Create(model, ct);

            return Ok(resourceId);
        }
    }
}