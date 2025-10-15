using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using QuranPreservationSystem.Application.Interfaces;
using QuranPreservationSystem.Infrastructure.Identity;

namespace QuranPreservationSystem.Authorization;

public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public PermissionHandler(
        UserManager<ApplicationUser> userManager,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        // إذا كان المستخدم غير مسجل
        if (context.User?.Identity?.IsAuthenticated != true)
        {
            return;
        }

        // الحصول على المستخدم الحالي
        var user = await _userManager.GetUserAsync(context.User);
        if (user == null)
        {
            return;
        }

        // الحصول على الأدوار الخاصة بالمستخدم
        var roles = await _userManager.GetRolesAsync(user);
        
        // Admin له جميع الصلاحيات
        if (roles.Contains("Admin"))
        {
            context.Succeed(requirement);
            return;
        }

        // التحقق من الصلاحيات لكل دور
        foreach (var roleName in roles)
        {
            var role = await _unitOfWork.RolePermissions.GetRoleIdByNameAsync(roleName);
            if (role != null)
            {
                var hasPermission = await _unitOfWork.RolePermissions
                    .HasPermissionAsync(role, requirement.PermissionName, requirement.Action);

                if (hasPermission)
                {
                    context.Succeed(requirement);
                    return;
                }
            }
        }
    }
}

