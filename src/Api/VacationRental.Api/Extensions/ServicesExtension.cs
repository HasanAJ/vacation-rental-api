using Microsoft.Extensions.DependencyInjection;
using VacationRental.Api.Filters;
using VacationRental.Core.Interfaces.Adapters;
using VacationRental.Core.Interfaces.Managers;
using VacationRental.Core.Interfaces.Repositories;
using VacationRental.Core.Interfaces.Repositories.Shared;
using VacationRental.Core.Interfaces.Services;
using VacationRental.Core.Interfaces.UnitOfWork;
using VacationRental.Core.Interfaces.Validators;
using VacationRental.Core.Services.Adapters;
using VacationRental.Core.Services.Managers;
using VacationRental.Core.Services.Services;
using VacationRental.Core.Services.Validators;
using VacationRental.Infrastructure.Data.Repositories;
using VacationRental.Infrastructure.Data.Repositories.Shared;
using VacationRental.Infrastructure.Data.UnitOfWork;

namespace VacationRental.Api.Extensions
{
    public static class ServicesExtension
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            RegisterCore(services);
            RegisterInfrastructure(services);

            return services;
        }

        private static IServiceCollection RegisterInfrastructure(this IServiceCollection services)
        {
            // UOW
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Repositories
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IRentalRepository, RentalRepository>();
            services.AddScoped<IUnitRepository, UnitRepository>();

            return services;
        }

        private static IServiceCollection RegisterCore(this IServiceCollection services)
        {
            // Filters
            services.AddSingleton<ExceptionFilter>();

            // Adapters
            services.AddSingleton<IMappingAdapter, MappingAdapter>();

            //Validators
            services.AddScoped<IBookingValidator, BookingValidator>();
            services.AddScoped<IUnitValidator, UnitValidator>();

            // Managers
            services.AddScoped<IUnitManager, UnitManager>();

            //Services
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IRentalService, RentalService>();
            services.AddScoped<ICalendarService, CalendarService>();

            return services;
        }
    }
}