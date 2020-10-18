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
            CreateMap<Booking, BookingDto>().ForMember(
                dest => dest.RentalId,
                opt => opt.MapFrom(src => src.Unit.RentalId)
            );
            CreateMap<BookingBindingDto, Booking>();

            CreateMap<Rental, RentalDto>().ForMember(
                dest => dest.Units,
                opt => opt.MapFrom(src => src.AllUnits.Count)
            );
            CreateMap<RentalBindingDto, Rental>();
        }
    }
}