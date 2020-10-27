using AutoMapper;
using VacationRental.Core.Common.Constants;
using VacationRental.Core.Models.Domain;
using VacationRental.Core.Models.Dtos.Booking;
using VacationRental.Core.Models.Dtos.Calendar;
using VacationRental.Core.Models.Dtos.Rental;

namespace VacationRental.Core.Provider
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MapRental();
            MapBooking();
        }

        private void MapRental()
        {
            CreateMap<Rental, RentalDto>().ForMember(
                    dest => dest.Units,
                    opt => opt.MapFrom(src => src.AllUnits.Count)
                );

            CreateMap<RentalBindingDto, Rental>();
        }

        private void MapBooking()
        {
            CreateMap<Booking, BookingDto>()
                .ForMember(
                    dest => dest.RentalId,
                    opt => opt.MapFrom(src => src.Unit.RentalId)
                );

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