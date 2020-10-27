using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using VacationRental.Core.Common.Constants;
using VacationRental.Core.Exceptions;
using VacationRental.Core.Models.Dtos.Shared;

namespace VacationRental.Api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(IWebHostEnvironment env, ILogger<ExceptionFilter> logger)
        {
            _env = env;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            Exception exception = context.Exception;

            _logger.LogError(exception, exception.Message);

            HttpStatusCode status = HttpStatusCode.InternalServerError;
            string code = ApiCodeConstants.SERVER_ERROR;

            if (exception is CustomException customException)
            {
                code = customException.Code;
                status = customException.HttpStatusCode;
            }

            bool showDetails = _env.IsDevelopment();

            context.Result = new JsonResult(new ApiErrorDto()
            {
                Code = code,
                Message = showDetails ? exception.Message : null,
                StackTrace = showDetails ? exception.StackTrace : null,
            });

            context.ExceptionHandled = true;

            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = (int)status;
            response.ContentType = "application/json";
        }
    }
}