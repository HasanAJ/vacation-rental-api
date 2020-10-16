using System.Threading;
using System.Threading.Tasks;
using VacationRental.Core.Models.Domain;
using VacationRental.Core.Models.Dtos.Rental;

namespace VacationRental.Core.Interfaces.Validators
{
    public interface IRentalValidator
    {
        Task Validate(Rental rental, RentalBindingDto model, CancellationToken ct);
    }
}