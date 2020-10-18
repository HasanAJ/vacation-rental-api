using System.Net;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Core.Common.Constants;
using VacationRental.Core.Exceptions;
using VacationRental.Core.Interfaces.Adapters;
using VacationRental.Core.Interfaces.Managers;
using VacationRental.Core.Interfaces.Services;
using VacationRental.Core.Interfaces.UnitOfWork;
using VacationRental.Core.Models.Domain;
using VacationRental.Core.Models.Dtos.Rental;
using VacationRental.Core.Models.Dtos.Shared;

namespace VacationRental.Core.Services
{
    public class RentalService : IRentalService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUnitManager _unitManager;
        private readonly IMappingAdapter _mapper;

        public RentalService(IUnitOfWork uow,
            IUnitManager unitManager,
            IMappingAdapter mapper)
        {
            _uow = uow;
            _unitManager = unitManager;
            _mapper = mapper;
        }

        public async Task<RentalDto> Get(int rentalId, CancellationToken ct)
        {
            Rental rental = await _uow.RentalRepository.Get(rentalId, ct);

            if (rental == null)
            {
                throw new CustomException(ApiCodeConstants.NOT_FOUND, ApiErrorMessageConstants.RENTAL_NOT_FOUND, HttpStatusCode.NotFound);
            }

            RentalDto rentalDto = _mapper.Map<RentalDto>(rental);

            return rentalDto;
        }

        public async Task<ResourceIdDto> Create(RentalBindingDto model, CancellationToken ct)
        {
            Rental rental = _mapper.Map<Rental>(model);

            _mapper.Map(model, rental);

            await _uow.RentalRepository.Add(rental, ct);

            for (int i = 0; i < model.Units; i++)
            {
                await _uow.UnitRepository.Add(new Unit()
                {
                    Rental = rental,
                    IsActive = true
                }, ct);
            }

            await _uow.Commit(ct);

            ResourceIdDto resourceIdDto = new ResourceIdDto()
            {
                Id = rental.Id
            };

            return resourceIdDto;
        }

        public async Task Update(int rentalId, RentalBindingDto model, CancellationToken ct)
        {
            Rental rental = await _uow.RentalRepository.Get(rentalId, ct);

            if (rental == null)
            {
                throw new CustomException(ApiCodeConstants.NOT_FOUND, ApiErrorMessageConstants.RENTAL_NOT_FOUND, HttpStatusCode.NotFound);
            }

            await _unitManager.HandleChange(rental, model, ct);

            _mapper.Map(model, rental);

            _uow.RentalRepository.Update(rental);

            await _uow.Commit(ct);
        }
    }
}