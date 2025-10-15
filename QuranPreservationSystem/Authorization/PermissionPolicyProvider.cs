using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace QuranPreservationSystem.Authorization;

public class PermissionPolicyProvider : IAuthorizationPolicyProvider
{
    private readonly DefaultAuthorizationPolicyProvider _fallbackPolicyProvider;

    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
    {
        _fallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    }

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith(PermissionAuthorizeAttribute.PolicyPrefix, StringComparison.OrdinalIgnoreCase))
        {
            // استخراج اسم الصلاحية والإجراء من اسم السياسة
            var parts = policyName.Substring(PermissionAuthorizeAttribute.PolicyPrefix.Length).Split('_');
            if (parts.Length >= 2)
            {
                var permissionName = parts[0];
                var action = parts[1];

                var policy = new AuthorizationPolicyBuilder();
                policy.AddRequirements(new PermissionRequirement(permissionName, action));
                return Task.FromResult<AuthorizationPolicy?>(policy.Build());
            }
        }

        return _fallbackPolicyProvider.GetPolicyAsync(policyName);
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
        return _fallbackPolicyProvider.GetDefaultPolicyAsync();
    }

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
    {
        return _fallbackPolicyProvider.GetFallbackPolicyAsync();
    }
}

