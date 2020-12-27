using System;
using System.Net;

namespace VacationRental.Common.Exceptions
{
    public class CustomException : Exception
    {
        public CustomException()
        {
        }

        public CustomException(string code, string message, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest) : base(message)
        {
            Code = code;
            HttpStatusCode = httpStatusCode;
        }

        public string Code { get; set; }

        public HttpStatusCode HttpStatusCode { get; set; }
    }
}