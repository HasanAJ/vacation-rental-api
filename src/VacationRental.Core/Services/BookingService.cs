using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Core.Common.Constants;
using VacationRental.Core.Exceptions;
using VacationRental.Core.Interfaces.Adapters;
using VacationRental.Core.Interfaces.Managers;
using VacationRental.Core.Interfaces.Services;
using VacationRental.Core.Interfaces.UnitOfWork;
using VacationRental.Core.Interfaces.Validators;
using VacationRental.Core.Models.Domain;
using VacationRental.Core.Models.Dtos.Booking;
using VacationRental.Core.Models.Dtos.Shared;

namespace VacationRental.Core.Services
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _uow;
        private readonly IBookingValidator _bookingValidator;
        private readonly IUnitManager _unitManager;
        private readonly IMappingAdapter _mapper;

        public BookingService(IUnitOfWork uow,
            IBookingValidator bookingValidator,
            IUnitManager unitManager,
            IMappingAdapter mapper)
        {
            _uow = uow;
            _bookingValidator = bookingValidator;
            _unitManager = unitManager;
            _mapper = mapper;
        }

        public async Task<BookingDto> Get(int bookingId, CancellationToken ct)
        {
            Booking booking = await _uow.BookingRepository.Find(bookingId, ct);

            if (booking == null)
            {
                throw new CustomException(ApiCodeConstants.NOT_FOUND, ApiErrorMessageConstants.BOOKING_NOT_FOUND, HttpStatusCode.NotFound);
            }

            BookingDto bookingDto = _mapper.Map<BookingDto>(booking);

            return bookingDto;
        }

        public async Task<ResourceIdDto> Create(BookingBindingDto model, CancellationToken ct)
        {
            Rental rental = await _uow.RentalRepository.Find(model.RentalId, ct);

            if (rental == null)
            {
                throw new CustomException(ApiCodeConstants.NOT_FOUND, ApiErrorMessageConstants.RENTAL_NOT_FOUND);
            }

            List<Booking> occupiedUnits = await _uow.BookingRepository.Get(model.RentalId, model.Start, model.Nights, rental.PreparationTimeInDays, ct);

            _bookingValidator.Validate(rental, occupiedUnits);

            Booking booking = _mapper.Map<Booking>(model);

            booking.UnitId = _unitManager.GetUnitId(rental, occupiedUnits);

            await _uow.BookingRepository.Add(booking, ct);

            await _uow.Commit(ct);

            ResourceIdDto resourceIdDto = new ResourceIdDto()
            {
                Id = booking.Id
            };

            return resourceIdDto;
        }
    }
}