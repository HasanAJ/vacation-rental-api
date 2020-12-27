using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Common.Constants;
using VacationRental.Common.Exceptions;
using VacationRental.Core.Dtos.Booking;
using VacationRental.Core.Entities;
using VacationRental.Core.Interfaces.Adapters;
using VacationRental.Core.Interfaces.Managers;
using VacationRental.Core.Interfaces.Services;
using VacationRental.Core.Interfaces.UnitOfWork;
using VacationRental.Core.Interfaces.Validators;
using VacationRental.Core.Services.Adapters;
using VacationRental.Core.Services.Mapping;
using VacationRental.Core.Services.Services;
using Xunit;

namespace VacationRental.UnitTests.Services
{
    public class BookingServiceTests
    {
        private readonly Mock<IUnitOfWork> _uow;
        private readonly Mock<IBookingValidator> _bookingValidator;
        private readonly Mock<IUnitManager> _unitManager;
        private readonly IMappingAdapter _mapper;

        private readonly IBookingService _bookingService;

        private readonly Booking booking = new Booking()
        {
            Id = 1,
            UnitId = 1,
            Start = new DateTime(2020, 1, 1),
            Nights = 2,
            Unit = new Unit()
            {
                Id = 1,
                RentalId = 1,
                IsActive = true
            }
        };

        public BookingServiceTests()
        {
            _uow = new Mock<IUnitOfWork>();
            _bookingValidator = new Mock<IBookingValidator>();
            _unitManager = new Mock<IUnitManager>();

            MapperConfiguration configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainToDtoMapping());
                cfg.AddProfile(new DtoToDomainMapping());
            });
            Mapper autoMapper = new Mapper(configuration);

            _mapper = new MappingAdapter(autoMapper);

            _bookingService = new BookingService(_uow.Object,
                _bookingValidator.Object,
                _unitManager.Object,
                _mapper);
        }

        [Fact]
        public async Task Get_Success()
        {
            _uow.Setup(x => x.BookingRepository.Get(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(booking));

            BookingDto expected = new BookingDto()
            {
                Id = 1,
                RentalId = 1,
                Start = new DateTime(2020, 1, 1),
                Nights = 2
            };

            BookingDto actual = await _bookingService.Get(1, new CancellationToken());

            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.RentalId, actual.RentalId);
            Assert.Equal(expected.Start, actual.Start);
            Assert.Equal(expected.Nights, actual.Nights);
        }

        [Fact]
        public async Task Get_Fail_NotFound()
        {
            _uow.Setup(x => x.BookingRepository.Find(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult((Booking)null));

            CustomException exception = await Assert.ThrowsAsync<CustomException>(() => _bookingService.Get(1, new CancellationToken()));

            Assert.Equal(ApiCodeConstants.NOT_FOUND, exception.Code);
        }

        [Fact]
        public async Task Create_Success()
        {
            Rental rental = new Rental()
            {
                Id = 1,
                AllUnits = new List<Unit>() { new Unit() { Id = 1, RentalId = 1, IsActive = true } },
                PreparationTimeInDays = 1
            };

            _uow.Setup(x => x.RentalRepository.Get(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(rental));
            _bookingValidator.Setup(x => x.Validate(It.IsAny<Rental>(), It.IsAny<List<Booking>>()));
            _unitManager.Setup(x => x.GetFreeUnitId(It.IsAny<int>(), It.IsAny<List<Booking>>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));
            _uow.Setup(x => x.BookingRepository.Add(It.IsAny<Booking>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _uow.Setup(x => x.Commit(It.IsAny<CancellationToken>())).Returns(Task.FromResult(It.IsAny<int>()));

            BookingBindingDto bookingBindingDto = new BookingBindingDto()
            {
                RentalId = 1,
                Start = new DateTime(2020, 1, 1),
                Nights = 2
            };

            await _bookingService.Create(bookingBindingDto, new CancellationToken());

            _uow.Verify(i => i.BookingRepository.Add(It.IsAny<Booking>(), It.IsAny<CancellationToken>()), Times.Once());
            _uow.Verify(i => i.Commit(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task Create_Fail_NotFound()
        {
            Rental rental = new Rental()
            {
                Id = 1,
                AllUnits = new List<Unit>() { new Unit() { Id = 1, RentalId = 1, IsActive = true } },
                PreparationTimeInDays = 1
            };

            _uow.Setup(x => x.RentalRepository.Get(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult((Rental)null));

            BookingBindingDto bookingBindingDto = new BookingBindingDto()
            {
                RentalId = 1,
                Start = new DateTime(2020, 1, 1),
                Nights = 2
            };

            CustomException exception = await Assert.ThrowsAsync<CustomException>(() => _bookingService.Create(bookingBindingDto, new CancellationToken()));

            Assert.Equal(ApiCodeConstants.NOT_FOUND, exception.Code);
        }
    }
}