﻿using System.Threading;
using System.Threading.Tasks;
using VacationRental.Core.Interfaces.Repositories;
using VacationRental.Core.Interfaces.UnitOfWork;
using VacationRental.Infrastructure.Context;
using VacationRental.Infrastructure.Repositories;

namespace VacationRental.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        private IRentalRepository _rentalRepository;
        private IBookingRepository _bookingRepository;
        private IUnitRepository _unitRepository;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
        }

        public IRentalRepository RentalRepository => _rentalRepository = _rentalRepository ?? new RentalRepository(_db);

        public IBookingRepository BookingRepository => _bookingRepository = _bookingRepository ?? new BookingRepository(_db);

        public IUnitRepository UnitRepository => _unitRepository = _unitRepository ?? new UnitRepository(_db);

        public async Task<int> Commit(CancellationToken ct)
        {
            return await _db.SaveChangesAsync(ct);
        }

        public async Task Dispose()
        {
            await _db.DisposeAsync();
        }
    }
}