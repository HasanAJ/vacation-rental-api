using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Core.Common.Constants;
using VacationRental.Core.Exceptions;
using VacationRental.Core.Interfaces.Services;
using VacationRental.Core.Interfaces.UnitOfWork;
using VacationRental.Core.Models.Domain;
using VacationRental.Core.Models.Dtos.Calendar;
using VacationRental.Core.Services;
using Xunit;

namespace VacationRental.UnitTests.Services
{
    public class CalendarServiceTests
    {
        private readonly Mock<IUnitOfWork> _uow;

        private readonly ICalendarService _calendarService;

        private readonly Rental rental = new Rental()
        {
            Id = 1,
            AllUnits = new List<Unit>() { new Unit() { Id = 1, RentalId = 1, IsActive = true } },
            PreparationTimeInDays = 1
        };

        private readonly List<Booking> bookings = new List<Booking>()
        {
            new Booking()
            {
                Id = 1,
                UnitId = 1,
                Start = new DateTime(2020, 1, 1),
                Nights = 2
            },
            new Booking()
            {
                Id = 2,
                UnitId = 2,
                Start = new DateTime(2020, 1, 2),
                Nights = 2
            },
            new Booking()
            {
                Id = 3,
                UnitId = 1,
                Start = new DateTime(2020, 1, 5),
                Nights = 1
            }
        };

        public CalendarServiceTests()
        {
            _uow = new Mock<IUnitOfWork>();

            _calendarService = new CalendarService(_uow.Object);
        }

        [Fact]
        public async Task Get_Success()
        {
            _uow.Setup(x => x.RentalRepository.Get(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(rental));
            _uow.Setup(x => x.BookingRepository.Get(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(bookings));

            CalendarDto expected = new CalendarDto()
            {
                RentalId = 1,
                Dates = new List<CalendarDateDto>()
                {
                    new CalendarDateDto()
                    {
                        Date = new DateTime(2020, 1, 1),
                        Bookings = new List<CalendarBookingDto>()
                        {
                            new CalendarBookingDto()
                            {
                                Id = 1,
                                Unit = 1
                            }
                        },
                        PreparationTimes = new List<PreparationTimeDto>()
                        {
                        }
                    },
                    new CalendarDateDto()
                    {
                        Date = new DateTime(2020, 1, 2),
                        Bookings = new List<CalendarBookingDto>()
                        {
                            new CalendarBookingDto()
                            {
                                Id = 1,
                                Unit = 1
                            },
                            new CalendarBookingDto()
                            {
                                Id = 2,
                                Unit = 2
                            }
                        },
                        PreparationTimes = new List<PreparationTimeDto>()
                        {
                        }
                    },
                    new CalendarDateDto()
                    {
                        Date = new DateTime(2020, 1, 3),
                        Bookings = new List<CalendarBookingDto>()
                        {
                            new CalendarBookingDto()
                            {
                                Id = 2,
                                Unit = 2
                            }
                        },
                        PreparationTimes = new List<PreparationTimeDto>()
                        {
                            new PreparationTimeDto()
                            {
                                Unit = 1
                            }
                        }
                    },
                    new CalendarDateDto()
                    {
                        Date = new DateTime(2020, 1, 4),
                        Bookings = new List<CalendarBookingDto>()
                        {
                        },
                        PreparationTimes = new List<PreparationTimeDto>()
                        {
                            new PreparationTimeDto()
                            {
                                Unit = 2
                            }
                        }
                    },
                    new CalendarDateDto()
                    {
                        Date = new DateTime(2020, 1, 5),
                        Bookings = new List<CalendarBookingDto>()
                        {
                            new CalendarBookingDto()
                            {
                                Id = 3,
                                Unit = 1
                            }
                        },
                        PreparationTimes = new List<PreparationTimeDto>()
                        {
                        }
                    }
                }
            };

            CalendarDto actual = await _calendarService.Get(1, new DateTime(2020, 1, 1), 5, new CancellationToken());

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task Get_Fail_NotFound()
        {
            _uow.Setup(x => x.RentalRepository.Get(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult((Rental)null));

            CustomException exception = await Assert.ThrowsAsync<CustomException>(() => _calendarService.Get(1, new DateTime(2020, 1, 1), 4, new CancellationToken()));

            Assert.Equal(ApiCodeConstants.NOT_FOUND, exception.Code);
        }
    }
}