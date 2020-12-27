using System;
using System.ComponentModel.DataAnnotations;

namespace VacationRental.Common.Attributes
{
    public class FutureDateAttribute : ValidationAttribute
    {
        public FutureDateAttribute()
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            DateTime dateValue = (DateTime)value;

            if (dateValue.Date >= DateTime.UtcNow.Date)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage);
        }
    }
}