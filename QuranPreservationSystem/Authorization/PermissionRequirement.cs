using Microsoft.AspNetCore.Authorization;

namespace QuranPreservationSystem.Authorization;

public class PermissionRequirement : IAuthorizationRequirement
{
    public string PermissionName { get; }
    public string Action { get; }

    public PermissionRequirement(string permissionName, string action)
    {
        PermissionName = permissionName;
        Action = action;
    }
}

