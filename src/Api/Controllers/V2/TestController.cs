using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.V2;

[ApiVersion(2.0)]
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
