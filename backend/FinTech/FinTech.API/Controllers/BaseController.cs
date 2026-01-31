using FinTech.Application.common;
using Microsoft.AspNetCore.Mvc;

namespace FinTech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected ActionResult CreateProblemDetails(Error error)
        {
            var statusCode = error.Type switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError

            };

            var title = error.Type switch
            {
                ErrorType.Validation => "Bad Request",
                ErrorType.NotFound => "Not Found",
                ErrorType.Conflict => "Conflict",
                _ => "Server Error"
            };
            return Problem(
                detail: error.Description,
                instance: Request.Path,
                statusCode: statusCode,
                title: title
                );
        }

    }
}
