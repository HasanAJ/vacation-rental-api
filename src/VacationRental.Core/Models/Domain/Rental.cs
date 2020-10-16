using System.Collections.Generic;
using VacationRental.Core.Models.Domain.Shared;

namespace VacationRental.Core.Models.Domain
{
    public class Rental : BaseEntity
    {
        public int Units { get; set; }
        public int PreparationTimeInDays { get; set; }
        public virtual IEnumerable<Booking> Bookings { get; set; }
    }
}