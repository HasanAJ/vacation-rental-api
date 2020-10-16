using System;
using System.Collections.Generic;

namespace VacationRental.Core.Models.Dtos.Calendar
{
    public class CalendarDateDto
    {
        public DateTime Date { get; set; }
        public List<CalendarBookingDto> Bookings { get; set; }
        public List<PreparationTimeDto> PreparationTimes { get; set; }
    }
}