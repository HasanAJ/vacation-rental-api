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

        [Theory]
        [InlineData("2019-01-01", false)]
        [InlineData("2020-09-30", false)]
        [InlineData("2025-12-30", true)]
        public void IsValid(string date, bool isValid)
        {
            Assert.Equal(isValid, _futureDateAttribute.IsValid(DateTime.Parse(date).Date));
        }
    }
}