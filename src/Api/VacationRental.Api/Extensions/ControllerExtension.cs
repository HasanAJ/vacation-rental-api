using Microsoft.Extensions.DependencyInjection;
using VacationRental.Api.Filters;

namespace VacationRental.Api.Extensions
{
    public static class ControllerExtension
    {
        public static IServiceCollection AddCustomControllers(this IServiceCollection services)
        {
            services
                .AddControllers(options =>
                {
                    options.Filters.Add(typeof(ExceptionFilter));
                })
                .AddJsonOptions(x => x.JsonSerializerOptions.IgnoreNullValues = true);

            return services;
        }
    }
}