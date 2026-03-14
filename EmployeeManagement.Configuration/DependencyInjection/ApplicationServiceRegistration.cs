using EmployeeManagement.Application.Services.Implementations;
using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using EmployeeManagement.Application.Mappers;

namespace EmployeeManagement.Configuration.DependencyInjection;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(typeof(MappingProfile).Assembly);

        // FluentValidation
        services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();

        // Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IDesignationService, DesignationService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<ISalaryService, SalaryService>();
        services.AddScoped<IReportService, ReportService>();

        return services;
    }
}