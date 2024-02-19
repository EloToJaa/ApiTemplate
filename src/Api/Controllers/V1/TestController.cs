using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.V1;

[ApiVersion(1.0)]
public class TestController : ApiController
{
    [AllowAnonymous]
    [HttpGet("test")]
    public async Task<IActionResult> Test()
    {
        await Task.CompletedTask;
        return Ok("Test");
    }
}
