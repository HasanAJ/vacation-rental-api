using AutoMapper;
using VacationRental.Core.Interfaces.Adapters;

namespace VacationRental.Core.Adapters
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
    }
}