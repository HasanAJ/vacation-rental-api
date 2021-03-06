﻿using System.ComponentModel.DataAnnotations;

namespace VacationRental.Core.Dtos.Rental
{
    public class RentalBindingDto
    {
        [Range(1, int.MaxValue)]
        public int Units { get; set; }

        public int PreparationTimeInDays { get; set; }
    }
}