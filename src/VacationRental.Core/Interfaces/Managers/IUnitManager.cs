using System.Collections.Generic;
using VacationRental.Core.Models.Domain;

namespace VacationRental.Core.Interfaces.Managers
{
    public interface IUnitManager
    {
        int GetUnitId(Rental rental, List<Booking> occupiedUnits);
    }
}