using API.Errors;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            services.AddScoped<iTokenService, TokenService>();
            services.AddScoped<iOrderService, OrderService>();
            services.AddScoped<iUnitOfWork, UnitOfWork>();
            services.AddScoped<iProductRepository, ProductRepository>();
            services.AddScoped<iBasketRepository, BasketRepository>();
            services.AddScoped(typeof(iGenericRepository<>), (typeof(GenericRepository<>)));

            services.Configure<ApiBehaviorOptions>(options =>
            {
            options.InvalidModelStateResponseFactory = actionContext => 
            {
                var errors = actionContext.ModelState
                .Where(e => e.Value.Errors.Count > 0)
                .SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage).ToArray();

                var errorResponse = new ApiValidationErrorResponse 
                {
                Errors = errors
                };

                return new BadRequestObjectResult(errorResponse);
                
            };
        });

        return services;

        }
    }
}