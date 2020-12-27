using VacationRental.Core.Dtos.Booking;

namespace VacationRental.Core.Interfaces.Adapters
{
    public interface IMappingAdapter
    {
        TDestination Map<TDestination>(object source);

        TDestination Map<TSource, TDestination>(TSource source, TDestination destination);

        Booking MapBookingBindingDto<Booking>(BookingBindingDto source, int unitId);
    }
}