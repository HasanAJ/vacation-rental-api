using AutoMapper;
using VacationRental.Common.Constants;
using VacationRental.Core.Dtos.Booking;
using VacationRental.Core.Interfaces.Adapters;

namespace VacationRental.Core.Services.Adapters
{
    public class MappingAdapter : IMappingAdapter
    {
        private readonly IMapper _mapper;

        public MappingAdapter(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TDestination Map<TDestination>(object source)
        {
            return _mapper.Map<TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return _mapper.Map(source, destination);
        }

        public Booking MapBookingBindingDto<Booking>(BookingBindingDto source, int unitId)
        {
            return _mapper.Map<Booking>(source, opt =>
            {
                opt.Items[MappingItemCodes.UNIT_ID] = unitId;
            });
        }
    }
}