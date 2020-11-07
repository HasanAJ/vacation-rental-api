using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace VacationRental.Api.Extensions
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddDocs(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Vacation Rental API", Version = "v1" });

                c.EnableAnnotations();
            });

            return services;
        }

        public static IApplicationBuilder UseDocs(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = "Vacation Rental API - Docs";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Vacation Rental API");
                c.RoutePrefix = "docs";
            });

            return app;
        }
    }
}