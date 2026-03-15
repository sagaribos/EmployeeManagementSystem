using EmployeeManagement.Application.DTOs.Request.User;
using FluentValidation;

namespace EmployeeManagement.Application.Validators;

public class AssignRoleValidator : AbstractValidator<AssignRoleRequest>
{
    public AssignRoleValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty().WithMessage("Role is required");
    }
}