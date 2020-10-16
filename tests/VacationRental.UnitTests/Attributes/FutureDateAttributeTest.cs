using System;
using VacationRental.Core.Attributes;
using Xunit;

namespace AuthService.Core.UnitTests.Attributes
{
    public class FutureDateAttributeTest
    {
        private readonly FutureDateAttribute _futureDateAttribute;

        public FutureDateAttributeTest()
        {
            _futureDateAttribute = new FutureDateAttribute();
        }

        [Fact]
        public void Requirede_Success()
        {
            Assert.True(_futureDateAttribute.IsValid(DateTime.UtcNow.AddDays(1).Date));
        }

        [Fact]
        public void Required_Fail()
        {
            Assert.False(_futureDateAttribute.IsValid(new DateTime(2002, 1, 1)));
        }
    }
}