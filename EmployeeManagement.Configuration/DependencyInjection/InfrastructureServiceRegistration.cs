using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Infrastructure.Services;
using EmployeeManagement.Infrastructure.PasswordHasher;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagement.Configuration.DependencyInjection;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services)
    {
        // Token Service
        services.AddScoped<ITokenService, TokenService>();

        // Audit Log Service
        services.AddScoped<IAuditLogService, AuditLogService>();

        // Password Hasher
        services.AddScoped<PasswordHasher>();

        return services;
    }
}