using EmployeeManagement.Application.DTOs.Request.Designation;
using FluentValidation;

namespace EmployeeManagement.Application.Validators;

public class UpdateDesignationValidator : AbstractValidator<UpdateDesignationRequest>
{
    public UpdateDesignationValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Designation title is required")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters");

        RuleFor(x => x.DepartmentId)
            .NotEmpty().WithMessage("Department is required");
    }
}