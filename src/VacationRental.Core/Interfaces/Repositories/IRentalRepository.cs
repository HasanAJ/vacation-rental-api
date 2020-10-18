﻿using System.Threading;
using System.Threading.Tasks;
using VacationRental.Core.Interfaces.Repositories.Shared;
using VacationRental.Core.Models.Domain;

namespace VacationRental.Core.Interfaces.Repositories
{
    public interface IRentalRepository : IRepository<Rental>
    {
        Task<Rental> Get(int rentalId, CancellationToken ct);
    }
}