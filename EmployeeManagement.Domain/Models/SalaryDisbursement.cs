using EmployeeManagement.Domain.Common;
using EmployeeManagement.Domain.Enums;

namespace EmployeeManagement.Domain.Models;

public class SalaryDisbursement : BaseEntity
{
    public Guid EmployeeId { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal Amount { get; set; }
    public decimal? Adjustment { get; set; }
    public SalaryStatus Status { get; set; }
    public DateTime? DisbursedAt { get; set; }

    // Navigation Properties
    public Employee Employee { get; set; } = null!;
}