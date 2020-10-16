using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Api.Controllers.Shared;
using VacationRental.Core.Interfaces.Services;
using VacationRental.Core.Models.Dtos.Rental;
using VacationRental.Core.Models.Dtos.Shared;

namespace VacationRental.Api.Controllers
{
    public class RentalsController : BaseController
    {
        private readonly IRentalService _rentalService;

        public RentalsController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        [HttpGet("{rentalId:int}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(RentalDto))]
        public async Task<IActionResult> Get(int rentalId, CancellationToken ct)
        {
            RentalDto rental = await _rentalService.Get(rentalId, ct);

            return Ok(rental);
        }

        [HttpPost("")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(ResourceIdDto))]
        public async Task<IActionResult> Post(RentalBindingDto model, CancellationToken ct)
        {
            ResourceIdDto resourceId = await _rentalService.Create(model, ct);

            return Ok(resourceId);
        }

        [HttpPut("{rentalId:int}")]
        public async Task<IActionResult> Update(int rentalId, RentalBindingDto model, CancellationToken ct)
        {
            await _rentalService.Update(rentalId, model, ct);

            return Ok();
        }
    }
}