using Api.Common.Http;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;

namespace Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/[controller]")]
public class ApiController : ControllerBase
{
    protected IActionResult Problem(List<Error> errors)
    {
        if (errors.Count == 0)
        {
            return Problem();
        }

        if (errors.All(error => error.Type == ErrorType.Validation))
        {
            return ValidationProblem(errors);
        }

        HttpContext.Items.Add(HttpContextItemKeys.Errors, errors);

        return Problem(errors[0]);
    }

    private IActionResult Problem(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            ErrorType.Unexpected => StatusCodes.Status500InternalServerError,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(statusCode: statusCode, title: error.Description);
    }

    private IActionResult ValidationProblem(List<Error> errors)
    {
        var modelState = new ModelStateDictionary();

        foreach (var error in errors)
        {
            modelState.AddModelError(
                error.Code,
                error.Description);
        }
        return ValidationProblem(modelState);
    }

    protected Guid? UserId
    {
        get
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid? userIdGuid = userId is null ? null : new Guid(userId);
            return userIdGuid;
        }
    }
}
