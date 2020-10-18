using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Linq;
using VacationRental.Api.Filters;
using VacationRental.Core.Adapters;
using VacationRental.Core.Common.Constants;
using VacationRental.Core.Interfaces.Adapters;
using VacationRental.Core.Interfaces.Managers;
using VacationRental.Core.Interfaces.Repositories;
using VacationRental.Core.Interfaces.Repositories.Shared;
using VacationRental.Core.Interfaces.Services;
using VacationRental.Core.Interfaces.UnitOfWork;
using VacationRental.Core.Interfaces.Validators;
using VacationRental.Core.Managers;
using VacationRental.Core.Models.Dtos.Shared;
using VacationRental.Core.Services;
using VacationRental.Core.Validators;
using VacationRental.Infrastructure.Context;
using VacationRental.Infrastructure.Repositories;
using VacationRental.Infrastructure.Repositories.Shared;
using VacationRental.Infrastructure.UnitOfWork;

namespace VacationRental.Api.Extensions
{
    public static class ServicesExtension
    {
        public static void RegisterInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase(databaseName: "VacationRental"));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IRentalRepository, RentalRepository>();
            services.AddScoped<IUnitRepository, UnitRepository>();
        }

        public static void RegisterCore(this IServiceCollection services)
        {
            services.AddSingleton<ExceptionFilter>();

            services.AddSingleton<IMappingAdapter, MappingAdapter>();

            services.AddScoped<IBookingValidator, BookingValidator>();
            services.AddScoped<IUnitValidator, UnitValidator>();

            services.AddScoped<IUnitManager, UnitManager>();

            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IRentalService, RentalService>();
            services.AddScoped<ICalendarService, CalendarService>();
        }

        public static void ConfigureControllers(this IServiceCollection services)
        {
            services
                .AddControllers(options =>
                {
                    options.Filters.Add(typeof(ExceptionFilter));
                })
                .AddJsonOptions(x => x.JsonSerializerOptions.IgnoreNullValues = true);

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
        }

        public static void ConfigureDocs(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Vacation Rental API", Version = "v1" });

                c.EnableAnnotations();
            });
        }
    }
}