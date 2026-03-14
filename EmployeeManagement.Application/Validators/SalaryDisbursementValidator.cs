using EmployeeManagement.Application.DTOs.Request.Salary;
using FluentValidation;

namespace EmployeeManagement.Application.Validators;

public class SalaryDisbursementValidator : AbstractValidator<SalaryDisbursementRequest>
{
    public SalaryDisbursementValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("Employee is required");

        RuleFor(x => x.Month)
            .InclusiveBetween(1, 12).WithMessage("Month must be between 1 and 12");

        RuleFor(x => x.Year)
            .GreaterThan(2000).WithMessage("Year must be greater than 2000")
            .LessThanOrEqualTo(DateTime.UtcNow.Year).WithMessage("Year cannot be in the future");
    }
}