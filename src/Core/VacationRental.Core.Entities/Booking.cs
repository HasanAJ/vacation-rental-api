using System;
using VacationRental.Core.Entities.Shared;

namespace VacationRental.Core.Entities
{
    public class Booking : BaseEntity
    {
        public int UnitId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
        public virtual Unit Unit { get; set; }

        public DateTime End => Start.AddDays(Nights);
    }
}