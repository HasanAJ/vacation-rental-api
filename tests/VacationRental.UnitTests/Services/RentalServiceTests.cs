using AutoMapper;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Common.Constants;
using VacationRental.Common.Exceptions;
using VacationRental.Core.Dtos.Rental;
using VacationRental.Core.Dtos.Shared;
using VacationRental.Core.Entities;
using VacationRental.Core.Interfaces.Adapters;
using VacationRental.Core.Interfaces.Managers;
using VacationRental.Core.Interfaces.Services;
using VacationRental.Core.Interfaces.UnitOfWork;
using VacationRental.Core.Services.Adapters;
using VacationRental.Core.Services.Mapping;
using VacationRental.Core.Services.Services;
using Xunit;

namespace VacationRental.UnitTests.Services
{
    public class RentalServiceTests
    {
        private readonly Mock<IUnitOfWork> _uow;
        private readonly Mock<IUnitManager> _unitManager;
        private readonly IMappingAdapter _mapper;

        private readonly IRentalService _rentalService;

        private readonly Rental rental = new Rental()
        {
            Id = 1,
            AllUnits = new List<Unit>() { new Unit() { Id = 1, RentalId = 1, IsActive = true } },
            PreparationTimeInDays = 1
        };

        public RentalServiceTests()
        {
            _uow = new Mock<IUnitOfWork>();
            _unitManager = new Mock<IUnitManager>();

            MapperConfiguration configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainToDtoMapping());
                cfg.AddProfile(new DtoToDomainMapping());
            });
            Mapper autoMapper = new Mapper(configuration);
            _mapper = new MappingAdapter(autoMapper);

            _rentalService = new RentalService(_uow.Object,
                _unitManager.Object,
                _mapper);
        }

        [Fact]
        public async Task Get_Success()
        {
            _uow.Setup(x => x.RentalRepository.Get(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(rental));

            RentalDto expected = new RentalDto()
            {
                Id = 1,
                Units = 1,
                PreparationTimeInDays = 1
            };

            RentalDto actual = await _rentalService.Get(rental.Id, new CancellationToken());

            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Units, actual.Units);
        }

        [Fact]
        public async Task Get_Fail_NotFound()
        {
            _uow.Setup(x => x.RentalRepository.Get(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult((Rental)null));

            CustomException exception = await Assert.ThrowsAsync<CustomException>(() => _rentalService.Get(rental.Id, new CancellationToken()));

            Assert.Equal(ApiCodeConstants.NOT_FOUND, exception.Code);
        }

        [Fact]
        public async Task Create_Success()
        {
            _uow.Setup(x => x.RentalRepository.Add(It.IsAny<Rental>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _uow.Setup(x => x.UnitRepository.Add(It.IsAny<Unit>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _uow.Setup(x => x.Commit(It.IsAny<CancellationToken>())).Returns(Task.FromResult(It.IsAny<int>()));

            RentalBindingDto rentalBindingDto = new RentalBindingDto()
            {
                Units = 2,
                PreparationTimeInDays = 1
            };

            ResourceIdDto actual = await _rentalService.Create(rentalBindingDto, new CancellationToken());

            _uow.Verify(i => i.RentalRepository.Add(It.IsAny<Rental>(), It.IsAny<CancellationToken>()), Times.Once());
            _uow.Verify(i => i.Commit(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task Update_Success()
        {
            _uow.Setup(x => x.RentalRepository.Get(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(rental));
            _unitManager.Setup(x => x.HandleChange(It.IsAny<Rental>(), It.IsAny<RentalBindingDto>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _uow.Setup(x => x.RentalRepository.Update(It.IsAny<Rental>()));
            _uow.Setup(x => x.Commit(It.IsAny<CancellationToken>())).Returns(Task.FromResult(It.IsAny<int>()));

            RentalBindingDto rentalBindingDto = new RentalBindingDto()
            {
                Units = 3,
                PreparationTimeInDays = 2
            };

            await _rentalService.Update(rental.Id, rentalBindingDto, new CancellationToken());

            _uow.Verify(i => i.RentalRepository.Update(It.IsAny<Rental>()), Times.Once());
            _uow.Verify(i => i.Commit(It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}