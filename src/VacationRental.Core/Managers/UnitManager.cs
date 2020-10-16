using System.Collections.Generic;
using System.Linq;
using VacationRental.Core.Interfaces.Managers;
using VacationRental.Core.Models.Domain;

namespace VacationRental.Core.Managers
{
    public class UnitManager : IUnitManager
    {
        public UnitManager()
        {
        }

        public int GetUnitId(Rental rental, List<Booking> occupiedUnits)
        {
            int unitId = 1;

            if (occupiedUnits.Any())
            {
                IEnumerable<int> unitIds = Enumerable.Range(1, rental.Units);

                List<int> occupiedUnitIds = occupiedUnits
                    .Select(i => i.UnitId)
                    .ToList();

                IEnumerable<int> freeUnitIds = unitIds
                    .Except(occupiedUnitIds)
                    .ToList();

                unitId = freeUnitIds
                    .OrderBy(i => i)
                    .FirstOrDefault();
            }

            return unitId;
        }
    }
}