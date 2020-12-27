using System.Collections.Generic;
using VacationRental.Core.Entities.Shared;

namespace VacationRental.Core.Entities
{
    public class Rental : BaseEntity
    {
        public Rental()
        {
            AllUnits = new List<Unit>();
        }

        public int PreparationTimeInDays { get; set; }
        public virtual ICollection<Unit> AllUnits { get; set; }
    }
}