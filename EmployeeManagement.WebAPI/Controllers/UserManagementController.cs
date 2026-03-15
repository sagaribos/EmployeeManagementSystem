using EmployeeManagement.Application.DTOs.Request.User;
using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Shared.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.WebAPI.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UserManagementController : ControllerBase
{
    private readonly IUserManagementService _userManagementService;

    public UserManagementController(IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _userManagementService.GetAllAsync();
        return Ok(ResponseHelper.Success(result, "Users retrieved successfully"));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _userManagementService.GetByIdAsync(id);
        return Ok(ResponseHelper.Success(result, "User retrieved successfully"));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var result = await _userManagementService.CreateAsync(request);
        return Ok(ResponseHelper.Success(result, "User created successfully"));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateUserRequest request)
    {
        var result = await _userManagementService.UpdateAsync(id, request);
        return Ok(ResponseHelper.Success(result, "User updated successfully"));
    }

    [HttpPost("{id}/roles")]
    public async Task<IActionResult> AssignRole(
        Guid id,
        [FromBody] AssignRoleRequest request)
    {
        await _userManagementService.AssignRoleAsync(id, request);
        return Ok(ResponseHelper.Success<object>(null!, "Role assigned successfully"));
    }

    [HttpDelete("{id}/roles/{roleId}")]
    public async Task<IActionResult> RemoveRole(Guid id, Guid roleId)
    {
        await _userManagementService.RemoveRoleAsync(id, roleId);
        return Ok(ResponseHelper.Success<object>(null!, "Role removed successfully"));
    }
}