using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Zitadel.Authentication;

namespace Api.Controllers.V2;

[ApiVersion(2.0)]
public class TestController : ApiController
{
    [Authorize]
    [HttpGet("test")]
    public object Test()
    {
        return Result();
    }

    private object Result() => new
    {
        Ping = "Pong",
        Timestamp = DateTime.Now,
        AuthType = User.Identity?.AuthenticationType,
        UserName = User.Identity?.Name,
        UserId = User.FindFirstValue(OidcClaimTypes.Subject),
        Claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList(),
        IsInAdminRole = User.IsInRole("Admin"),
        IsInUserRole = User.IsInRole("User"),
    };
}
