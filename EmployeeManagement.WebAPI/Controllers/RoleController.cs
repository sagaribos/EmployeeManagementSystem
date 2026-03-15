using EmployeeManagement.Application.DTOs.Request.Role;
using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Shared.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.WebAPI.Controllers;

[ApiController]
[Route("api/roles")]
[Authorize]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _roleService.GetAllAsync();
        return Ok(ResponseHelper.Success(result, "Roles retrieved successfully"));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _roleService.GetByIdAsync(id);
        return Ok(ResponseHelper.Success(result, "Role retrieved successfully"));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoleRequest request)
    {
        var result = await _roleService.CreateAsync(request);
        return Ok(ResponseHelper.Success(result, "Role created successfully"));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoleRequest request)
    {
        var result = await _roleService.UpdateAsync(id, request);
        return Ok(ResponseHelper.Success(result, "Role updated successfully"));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _roleService.DeleteAsync(id);
        return Ok(ResponseHelper.Success<object>(null!, "Role deleted successfully"));
    }

    [HttpPost("{id}/permissions")]
    public async Task<IActionResult> AssignPermission(
        Guid id,
        [FromBody] AssignPermissionRequest request)
    {
        await _roleService.AssignPermissionAsync(id, request);
        return Ok(ResponseHelper.Success<object>(null!, "Permission assigned successfully"));
    }

    [HttpDelete("{id}/permissions/{permissionId}")]
    public async Task<IActionResult> RemovePermission(Guid id, Guid permissionId)
    {
        await _roleService.RemovePermissionAsync(id, permissionId);
        return Ok(ResponseHelper.Success<object>(null!, "Permission removed successfully"));
    }
}