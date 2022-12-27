using Microsoft.Extensions.DependencyInjection;
using ShoppingCart.Application.Services;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IShoppingCartService, ShoppingCartService>();

            return services;
        }
    }
}
