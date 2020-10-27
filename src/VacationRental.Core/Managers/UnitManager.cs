using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

            if (occupiedUnits != null && occupiedUnits.Any())
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
                    .First();
            }
            else
            {
                unitId = allUnits
                    .Select(i => i.Id)
                    .OrderBy(i => i)
                    .First();
            }

            return unitId;
        }

        public async Task HandleChange(Rental rental, RentalBindingDto model, CancellationToken ct)
        {
            int totalUnits = rental.AllUnits.Count;

            List<Booking> bookings = new List<Booking>();

            bool isLessUnitsOrMorePrep = model.Units < totalUnits || model.PreparationTimeInDays > rental.PreparationTimeInDays;

            if (isLessUnitsOrMorePrep)
            {
                DateTime start = DateTime.UtcNow.Date;

                bookings = await _uow.BookingRepository.Get(rental.Id, start, ct);

                _unitValidator.Validate(model, bookings, start);
            }

            if (model.Units > totalUnits)
            {
                await HandleMoreUnits(rental, model.Units, ct);
            }
            else if (isLessUnitsOrMorePrep)
            {
                HandleLessUnits(rental.AllUnits.ToList(), bookings, model.Units);
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

        private void HandleLessUnits(List<Unit> allUnits, List<Booking> bookings, int newUnits)
        {
            List<Unit> freeUnits = new List<Unit>();

            List<Unit> occupiedUnits = bookings
                .Select(i => i.Unit)
                .ToList();

            freeUnits = allUnits
                    .Where(i => !occupiedUnits.Contains(i))
                    .ToList();

            if (freeUnits != null)
            {
                int unitsToDisable = allUnits.Count - newUnits;

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
}