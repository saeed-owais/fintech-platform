using FinTech.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinTech.Infrastructure
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(option =>
                option.UseSqlServer(connectionString)
            );

            services.AddScoped<IApplicationDbContext>
                (sp => sp.GetRequiredService<ApplicationDbContext>());


            return services;
        }
    }
}
