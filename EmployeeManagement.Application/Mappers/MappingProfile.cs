using AutoMapper;
using EmployeeManagement.Application.DTOs.Response.Department;
using EmployeeManagement.Application.DTOs.Response.Designation;
using EmployeeManagement.Application.DTOs.Response.Employee;
using EmployeeManagement.Application.DTOs.Response.Salary;
using EmployeeManagement.Domain.Models;

namespace EmployeeManagement.Application.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Department
        CreateMap<Department, DepartmentResponse>();

        // Designation
        CreateMap<Designation, DesignationResponse>()
            .ForMember(dest => dest.DepartmentName,
                opt => opt.MapFrom(src => src.Department.Name));

        // Employee
        CreateMap<Employee, EmployeeResponse>()
            .ForMember(dest => dest.DepartmentName,
                opt => opt.MapFrom(src => src.Department.Name))
            .ForMember(dest => dest.DesignationTitle,
                opt => opt.MapFrom(src => src.Designation.Title));

        // Salary
        CreateMap<SalaryDisbursement, SalaryDisbursementResponse>()
            .ForMember(dest => dest.EmployeeName,
                opt => opt.MapFrom(src =>
                    src.Employee.FirstName + " " + src.Employee.LastName));
    }
}