using System;
using VacationRental.Core.Models.Domain.Shared;

namespace VacationRental.Core.Models.Domain
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