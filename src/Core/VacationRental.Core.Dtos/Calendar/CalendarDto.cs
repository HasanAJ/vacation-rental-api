using System.Collections.Generic;

namespace VacationRental.Core.Dtos.Calendar
{
    public class CalendarDto
    {
        public int RentalId { get; set; }
        public List<CalendarDateDto> Dates { get; set; }
    }
}