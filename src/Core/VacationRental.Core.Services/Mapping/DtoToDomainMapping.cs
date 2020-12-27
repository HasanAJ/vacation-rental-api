using AutoMapper;
using VacationRental.Common.Constants;
using VacationRental.Core.Dtos.Booking;
using VacationRental.Core.Dtos.Rental;
using VacationRental.Core.Entities;

namespace VacationRental.Core.Services.Mapping
{
    public class DtoToDomainMapping : Profile
    {
        public DtoToDomainMapping()
        {
            CreateMap<RentalBindingDto, Rental>();

            CreateMap<BookingBindingDto, Booking>()
                .ForMember(
                    dest => dest.UnitId,
                    opt => opt.MapFrom(
                        (s, d, _, context) => context.Options.Items[MappingItemCodes.UNIT_ID]
                    )
                );
        }
    }
}