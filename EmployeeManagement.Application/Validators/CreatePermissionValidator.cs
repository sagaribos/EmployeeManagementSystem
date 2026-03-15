using EmployeeManagement.Application.DTOs.Request.Permission;
using FluentValidation;

namespace EmployeeManagement.Application.Validators;

public class CreatePermissionValidator : AbstractValidator<CreatePermissionRequest>
{
    public CreatePermissionValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Permission name is required")
            .MaximumLength(100).WithMessage("Permission name cannot exceed 100 characters");

        RuleFor(x => x.Endpoint)
            .NotEmpty().WithMessage("Endpoint is required")
            .MaximumLength(200).WithMessage("Endpoint cannot exceed 200 characters");

        RuleFor(x => x.HttpMethod)
            .IsInEnum().WithMessage("Invalid HTTP method");
    }
}