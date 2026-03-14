using EmployeeManagement.Domain.Enums;

namespace EmployeeManagement.Application.DTOs.Response.Salary;

public class SalaryDisbursementResponse
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal Amount { get; set; }
    public decimal? Adjustment { get; set; }
    public SalaryStatus Status { get; set; }
    public DateTime? DisbursedAt { get; set; }
}