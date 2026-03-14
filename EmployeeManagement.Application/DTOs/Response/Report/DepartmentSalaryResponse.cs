namespace EmployeeManagement.Application.DTOs.Response.Report;

public class DepartmentSalaryResponse
{
    public string DepartmentName { get; set; } = string.Empty;
    public decimal TotalSalary { get; set; }
    public int TotalEmployees { get; set; }
}