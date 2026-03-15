using EmployeeManagement.Application.DTOs.Request.Role;
using FluentValidation;

namespace EmployeeManagement.Application.Validators;

public class AssignPermissionValidator : AbstractValidator<AssignPermissionRequest>
{
    public AssignPermissionValidator()
    {
        RuleFor(x => x.PermissionId)
            .NotEmpty().WithMessage("Permission is required");
    }
}