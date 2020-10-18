using System.Collections.Generic;
using VacationRental.Core.Models.Domain.Shared;

namespace VacationRental.Core.Models.Domain
{
    public class Unit : BaseEntity
    {
        public Unit()
        {
            Bookings = new List<Booking>();
        }

        public int RentalId { get; set; }
        public bool IsActive { get; set; }
        public virtual Rental Rental { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}