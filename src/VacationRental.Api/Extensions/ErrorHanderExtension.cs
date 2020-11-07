using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using VacationRental.Core.Common.Constants;
using VacationRental.Core.Models.Dtos.Shared;

namespace VacationRental.Api.Extensions
{
    public static class ErrorHanderExtension
    {
        public static IServiceCollection AddInvalidModelHandler(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
                options.InvalidModelStateResponseFactory = context =>
                {
                    return new BadRequestObjectResult(new ApiErrorDto()
                    {
                        Code = ApiCodeConstants.BAD_REQUEST,
                        Errors = context.ModelState.Values
                                                    .SelectMany(x => x.Errors.Select(x => x.ErrorMessage))
                    });
                }
            );

            return services;
        }
    }
}