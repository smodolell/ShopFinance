using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ShopFinance.Application.Common.Interfaces;

namespace ShopFinance.Infrastructure.Authentication;

public class JwtKeyResolver
{
    private readonly IJwtConfigurationProvider _configProvider;
    private readonly ILogger<JwtKeyResolver> _logger;

    public JwtKeyResolver(
        IJwtConfigurationProvider configProvider,
        ILogger<JwtKeyResolver> logger)
    {
        _configProvider = configProvider;
        _logger = logger;
    }

    public async Task<SecurityKey> ResolveSigningKeyAsync(
        string token,
        SecurityToken securityToken,
        string kid,
        TokenValidationParameters validationParameters)
    {
        try
        {
            _logger.LogDebug("Resolving signing key for token validation");
            return await _configProvider.GetValidationKeyAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to resolve signing key");
            throw;
        }
    }

    public async Task<IEnumerable<SecurityKey>> ResolveIssuerSigningKeysAsync(
        string token,
        SecurityToken securityToken,
        string kid,
        TokenValidationParameters validationParameters)
    {
        try
        {
            _logger.LogDebug("Resolving issuer signing keys for token validation");
            var key = await _configProvider.GetValidationKeyAsync();
            return new[] { key };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to resolve issuer signing keys");
            throw;
        }
    }
}
public class BlazorAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    public Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
    {
        if (policy.Requirements.OfType<ApiAuthorizeRequirement>().Any() &&
            !authorizeResult.Succeeded)
        {
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
        }

        return next(context);
    }
}
public class ApiAuthorizeRequirement : IAuthorizationRequirement
{
}
public class ApiAuthorizeHandler : AuthorizationHandler<ApiAuthorizeRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ApiAuthorizeRequirement requirement)
    {
        context.Succeed(requirement);
    }
}
public class ApiAuthorizeAttribute : AuthorizeAttribute
{
    public ApiAuthorizeAttribute()
    {
        // 可以在这里设置默认的策略或其他属性
        Policy = "ApiAuthorizePolicy";
    }
}