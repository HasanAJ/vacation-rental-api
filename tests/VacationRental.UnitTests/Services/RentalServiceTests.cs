using AutoMapper;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Core.Adapters;
using VacationRental.Core.Common.Constants;
using VacationRental.Core.Exceptions;
using VacationRental.Core.Interfaces.Adapters;
using VacationRental.Core.Interfaces.Services;
using VacationRental.Core.Interfaces.UnitOfWork;
using VacationRental.Core.Interfaces.Validators;
using VacationRental.Core.Models.Domain;
using VacationRental.Core.Models.Dtos.Rental;
using VacationRental.Core.Models.Dtos.Shared;
using VacationRental.Core.Provider;
using VacationRental.Core.Services;
using Xunit;

namespace VacationRental.UnitTests.Services
{
    public class RentalServiceTests
    {
        private readonly Mock<IUnitOfWork> _uow;
        private readonly Mock<IRentalValidator> _rentalValidator;
        private readonly IMappingAdapter _mapper;

        private readonly IRentalService _rentalService;

        private readonly Rental rental = new Rental()
        {
            Id = 1,
            Units = 2,
            PreparationTimeInDays = 1
        };

        public RentalServiceTests()
        {
            _uow = new Mock<IUnitOfWork>();
            _rentalValidator = new Mock<IRentalValidator>();

            MappingProfile profile = new MappingProfile();
            MapperConfiguration configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            Mapper autoMapper = new Mapper(configuration);
            _mapper = new MappingAdapter(autoMapper);

            _rentalService = new RentalService(_uow.Object,
                _rentalValidator.Object,
                _mapper);
        }

        [Fact]
        public async Task Get_Success()
        {
            _uow.Setup(x => x.RentalRepository.Find(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(rental));

            RentalDto expected = new RentalDto()
            {
                Id = 1,
                Units = 2,
                PreparationTimeInDays = 1
            };

            RentalDto actual = await _rentalService.Get(1, new CancellationToken());

            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Units, actual.Units);
        }

        [Fact]
        public async Task Get_Fail_NotFound()
        {
            _uow.Setup(x => x.RentalRepository.Find(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult((Rental)null));

            CustomException exception = await Assert.ThrowsAsync<CustomException>(() => _rentalService.Get(1, new CancellationToken()));

            Assert.Equal(ApiCodeConstants.NOT_FOUND, exception.Code);
        }

        [Fact]
        public async Task Create_Success()
        {
            _uow.Setup(x => x.RentalRepository.Add(It.IsAny<Rental>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
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
            _uow.Setup(x => x.RentalRepository.Find(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(rental));
            _rentalValidator.Setup(x => x.Validate(It.IsAny<Rental>(), It.IsAny<RentalBindingDto>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _uow.Setup(x => x.RentalRepository.Update(It.IsAny<Rental>()));
            _uow.Setup(x => x.Commit(It.IsAny<CancellationToken>())).Returns(Task.FromResult(It.IsAny<int>()));

            RentalBindingDto rentalBindingDto = new RentalBindingDto()
            {
                Units = 3,
                PreparationTimeInDays = 2
            };

            await _rentalService.Update(1, rentalBindingDto, new CancellationToken());

            _uow.Verify(i => i.RentalRepository.Update(It.IsAny<Rental>()), Times.Once());
            _uow.Verify(i => i.Commit(It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}