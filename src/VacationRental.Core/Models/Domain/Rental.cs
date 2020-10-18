using System.Collections.Generic;
using System.Linq;
using VacationRental.Core.Models.Domain.Shared;

namespace VacationRental.Core.Models.Domain
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