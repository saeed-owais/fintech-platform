using FinTech.Application.Abstractions;
using FinTech.Infrastructure.Auth;
using FinTech.Infrastructure.Security;
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

            services.AddOptions<JwtOptions>()
                    .Bind(configuration.GetSection(JwtOptions.SectionName))
                    .Validate(o => !string.IsNullOrWhiteSpace(o.Key), "JWT Key is required")
                    .ValidateOnStart();

            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            services.AddScoped<IPasswordHasher, PasswordHasher>();

            return services;
        }
    }

}
