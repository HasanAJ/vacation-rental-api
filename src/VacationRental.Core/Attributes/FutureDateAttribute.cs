using System;
using System.ComponentModel.DataAnnotations;

namespace VacationRental.Core.Attributes
{
    public class FutureDateAttribute : ValidationAttribute
    {
        public FutureDateAttribute()
        {
        }

        public override bool IsValid(object value)
        {
            var dateValue = (DateTime)value;

            if (dateValue.Date >= DateTime.UtcNow.Date)
            {
                return true;
            }

            return false;
        }
    }
}