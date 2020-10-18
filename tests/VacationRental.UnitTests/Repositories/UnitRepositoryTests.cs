using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Core.Interfaces.Repositories;
using VacationRental.Core.Models.Domain;
using VacationRental.Infrastructure.Context;
using VacationRental.Infrastructure.Repositories;
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

            _dbContext.Rental.Add(rental);
            _dbContext.Unit.AddRange(units);

            _dbContext.SaveChanges();
        }

        [Fact]
        public async Task Get_Success()
        {
            List<Unit> actual = await _unitRepository.Get(rental.Id, new CancellationToken());

            List<Unit> expected = units
                .Where(i => i.RentalId == rental.Id)
               .ToList();

            Assert.Equal(expected.Count, actual.Count);

            for (int i = 0; i < actual.Count; i++)
            {
                Assert.Equal(expected[0].Id, actual[0].Id);
            }
        }
    }
}