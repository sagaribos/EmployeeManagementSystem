namespace EmployeeManagement.Application.DTOs.Response.Report;

public class SalarySummaryResponse
{
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal TotalDisbursed { get; set; }
    public int TotalEmployeesPaid { get; set; }
}