using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Core.Common.Constants;
using VacationRental.Core.Exceptions;
using VacationRental.Core.Interfaces.Managers;
using VacationRental.Core.Interfaces.UnitOfWork;
using VacationRental.Core.Interfaces.Validators;
using VacationRental.Core.Models.Domain;
using VacationRental.Core.Models.Dtos.Rental;

namespace VacationRental.Core.Managers
{
    public class UnitManager : IUnitManager
    {
        private readonly IUnitOfWork _uow;
        private readonly IUnitValidator _unitValidator;

        public UnitManager(IUnitOfWork uow,
            IUnitValidator unitValidator)
        {
            _uow = uow;
            _unitValidator = unitValidator;
        }

        public async Task<int> GetFreeUnitId(int rentalId, List<Booking> occupiedUnits, CancellationToken ct)
        {
            int unitId;

            List<Unit> allUnits = await _uow.UnitRepository.Get(rentalId, ct);

            if (occupiedUnits.Any())
            {
                List<int> occupiedUnitIds = occupiedUnits
                    .Select(i => i.UnitId)
                    .ToList();

                IEnumerable<int> freeUnitIds = allUnits
                    .Select(i => i.Id)
                    .Except(occupiedUnitIds)
                    .ToList();

                unitId = freeUnitIds
                    .OrderBy(i => i)
                    .FirstOrDefault();
            }
            else
            {
                unitId = allUnits
                    .Select(i => i.Id)
                    .OrderBy(i => i)
                    .FirstOrDefault();
            }

            return unitId;
        }

        public async Task HandleChange(Rental rental, RentalBindingDto model, CancellationToken ct)
        {
            int totalUnits = rental.AllUnits.Count;

            if (totalUnits < model.Units)
            {
                await HandleMoreUnits(rental, model.Units, ct);
            }
            else if (model.Units < totalUnits || model.PreparationTimeInDays > rental.PreparationTimeInDays)
            {
                DateTime start = DateTime.UtcNow;

                List<Booking> bookings = await _uow.BookingRepository.Get(rental.Id, start, ct);

                if (bookings != null)
                {
                    _unitValidator.Validate(model, bookings, start);
                }

                await HandleLessUnits(rental, bookings, model.Units, ct);
            }
        }

        private async Task HandleMoreUnits(Rental rental, int newUnits, CancellationToken ct)
        {
            int unitsToAdd = newUnits - rental.AllUnits.Count;

            for (int i = 0; i < unitsToAdd; i++)
            {
                await _uow.UnitRepository.Add(new Unit()
                {
                    Rental = rental,
                    IsActive = true
                }, ct);
            }
        }

        private async Task HandleLessUnits(Rental rental, List<Booking> bookings, int newUnits, CancellationToken ct)
        {
            List<Unit> freeUnits = new List<Unit>();

            List<Unit> allUnits = await _uow.UnitRepository.Get(rental.Id, ct);

            List<Unit> occupiedUnits = bookings
                .Select(i => i.Unit)
                .ToList();

            freeUnits = allUnits
                    .Where(i => !occupiedUnits.Contains(i))
                    .ToList();

            int unitsToDisable = rental.AllUnits.Count - newUnits;

            if (unitsToDisable > 0)
            {
                for (int i = 0; i < unitsToDisable; i++)
                {
                    freeUnits[i].IsActive = false;
                }
            }
        }
    }
}