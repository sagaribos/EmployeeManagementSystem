using EmployeeManagement.Domain.Enums;

namespace EmployeeManagement.Application.DTOs.Response.Employee;

public class EmployeeResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public DateTime JoiningDate { get; set; }
    public EmployeeStatus Status { get; set; }
    public decimal BaseSalary { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public string DesignationTitle { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}