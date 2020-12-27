using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Core.Entities;
using VacationRental.Core.Interfaces.Repositories;
using VacationRental.Infrastructure.Data.Context;
using VacationRental.Infrastructure.Data.Repositories;
using Xunit;

namespace VacationRental.UnitTests.Repositories
{
    public class UnitRepositoryTests
    {
        private static readonly Rental rental = new Rental
        {
            Id = 1,
            PreparationTimeInDays = 1
        };

        private readonly List<Unit> units = new List<Unit>()
        {
            new Unit()
            {
                Id = 1,
                Rental = rental,
                IsActive = true
            },
            new Unit()
            {
                Id = 2,
                Rental = rental,
                IsActive = true
            }
        };

        private readonly ApplicationDbContext _dbContext;
        private readonly IUnitRepository _unitRepository;

        public UnitRepositoryTests()
        {
            _dbContext = new MockDbContext().GetDbContext();
            _unitRepository = new UnitRepository(_dbContext);

            _dbContext.Rentals.Add(rental);
            _dbContext.Units.AddRange(units);

            _dbContext.SaveChanges();
        }

        [Fact]
        public async Task Get_Success()
        {
            List<Unit> actual = await _unitRepository.Get(rental.Id, new CancellationToken());

            List<Unit> expected = new List<Unit>()
            {
                new Unit()
                {
                    Id = 1,
                    RentalId = 1,
                    IsActive = true
                },
                new Unit()
                {
                    Id = 2,
                    RentalId = 1,
                    IsActive = true
                }
            };

            Assert.Equal(expected.Count, actual.Count);

            for (int i = 0; i < actual.Count; i++)
            {
                Assert.Equal(expected[0].Id, actual[0].Id);
                Assert.Equal(expected[0].RentalId, actual[0].Rental.Id);
                Assert.Equal(expected[0].IsActive, actual[0].IsActive);
            }
        }
    }
}