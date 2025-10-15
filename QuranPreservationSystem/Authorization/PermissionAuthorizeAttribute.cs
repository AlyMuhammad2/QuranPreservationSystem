using Microsoft.AspNetCore.Authorization;

namespace QuranPreservationSystem.Authorization;

public class PermissionAuthorizeAttribute : AuthorizeAttribute
{
    public const string PolicyPrefix = "Permission_";
    
    public PermissionAuthorizeAttribute(string permissionName, string action)
    {
        PermissionName = permissionName;
        Action = action;
        Policy = $"{PolicyPrefix}{permissionName}_{action}";
    }

    public string PermissionName { get; }
    public string Action { get; }
}

