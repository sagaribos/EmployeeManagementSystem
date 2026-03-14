namespace EmployeeManagement.Application.DTOs.Request.Salary;

public class SalaryDisbursementRequest
{
    public Guid EmployeeId { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal? Adjustment { get; set; }
}