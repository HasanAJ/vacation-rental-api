using AutoMapper;
using VacationRental.Core.Dtos.Booking;
using VacationRental.Core.Dtos.Rental;
using VacationRental.Core.Entities;

namespace VacationRental.Core.Application.Mapping
{
    public class DomainToDtoMapping : Profile
    {
        public DomainToDtoMapping()
        {
            CreateMap<Rental, RentalDto>().ForMember(
                    dest => dest.Units,
                    opt => opt.MapFrom(src => src.AllUnits.Count)
                );

            CreateMap<Booking, BookingDto>()
                .ForMember(
                    dest => dest.RentalId,
                    opt => opt.MapFrom(src => src.Unit.RentalId)
                );
        }
    }
}