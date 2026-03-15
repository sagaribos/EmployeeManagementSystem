using EmployeeManagement.Application.DTOs.Request.Role;
using FluentValidation;

namespace EmployeeManagement.Application.Validators;

public class CreateRoleValidator : AbstractValidator<CreateRoleRequest>
{
    public CreateRoleValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Role name is required")
            .MaximumLength(100).WithMessage("Role name cannot exceed 100 characters");
    }
}