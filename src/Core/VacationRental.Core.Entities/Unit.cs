using System.Collections.Generic;
using VacationRental.Core.Entities.Shared;

namespace VacationRental.Core.Entities
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