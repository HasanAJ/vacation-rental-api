using System;
using VacationRental.Common.Attributes;
using Xunit;

namespace VacationRental.UnitTests.Attributes
{
    public class FutureDateAttributeTest
    {
        private readonly FutureDateAttribute _futureDateAttribute;

        public FutureDateAttributeTest()
        {
            _futureDateAttribute = new FutureDateAttribute();
        }

        [Fact]
        public void IsValid()
        {
            Assert.True(_futureDateAttribute.IsValid(DateTime.UtcNow.Date));
            Assert.True(_futureDateAttribute.IsValid(DateTime.UtcNow.AddSeconds(5).Date));
            Assert.True(_futureDateAttribute.IsValid(DateTime.UtcNow.AddMinutes(5).Date));
            Assert.True(_futureDateAttribute.IsValid(DateTime.UtcNow.AddYears(5).Date));
            Assert.True(_futureDateAttribute.IsValid(DateTime.UtcNow.AddMinutes(-1).Date));

            Assert.False(_futureDateAttribute.IsValid(DateTime.UtcNow.AddDays(-1).Date));
            Assert.False(_futureDateAttribute.IsValid(DateTime.UtcNow.AddYears(-5).Date));
        }
    }
}