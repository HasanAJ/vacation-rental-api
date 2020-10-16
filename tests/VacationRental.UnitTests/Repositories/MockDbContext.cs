using Microsoft.EntityFrameworkCore;
using VacationRental.Infrastructure.Context;

namespace VacationRental.UnitTests.Repositories
{
    public class MockDbContext
    {
        public ApplicationDbContext GetDbContext()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
                            .UseInMemoryDatabase(databaseName: "VacationRental")
                            .Options;

            ApplicationDbContext dbContext = new ApplicationDbContext(options);

            return dbContext;
        }
    }
}