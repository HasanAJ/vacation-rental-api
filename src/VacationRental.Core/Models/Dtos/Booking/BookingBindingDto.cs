using System;
using System.ComponentModel.DataAnnotations;

namespace VacationRental.Core.Models.Dtos.Booking
{
    public class BookingBindingDto
    {
        private DateTime _startIgnoreTime;

        [Range(1, int.MaxValue)]
        public int RentalId { get; set; }

        public DateTime Start
        {
            get => _startIgnoreTime;
            set => _startIgnoreTime = value.Date;
        }

        [Range(1, int.MaxValue)]
        public int Nights { get; set; }
    }
}