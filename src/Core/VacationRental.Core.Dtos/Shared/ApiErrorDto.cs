using System.Collections.Generic;

namespace VacationRental.Core.Dtos.Shared
{
    public class ApiErrorDto
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}