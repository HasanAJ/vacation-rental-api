using AutoMapper;
using VacationRental.Core.Models.Domain;
using VacationRental.Core.Models.Dtos.Booking;
using VacationRental.Core.Models.Dtos.Rental;

namespace VacationRental.Core.Provider
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Booking, BookingDto>();
            CreateMap<BookingBindingDto, Booking>();

            CreateMap<Rental, RentalDto>();
            CreateMap<RentalBindingDto, Rental>();
        }
    }
}