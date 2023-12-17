using Core;
using Core.Models;
using Infrastructure.Loggers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected readonly IAppLogger _logger;
        protected BaseController(IAppLogger logger)
        {
            _logger = logger; 
        }
        protected IActionResult ResponseResult(ResponseResultObj responseResultObj)
        {
            if (responseResultObj.customOutcome.HasValue)
            {
                return ReturnCustomResponse(responseResultObj.customOutcome.Value,  responseResultObj.Message, responseResultObj.Result, responseResultObj.Uri);
            }

            return responseResultObj.IsSuccess
                ? Ok(new { Message = responseResultObj.Message, Data = responseResultObj.Result })
                : StatusCode(StatusCodes.Status500InternalServerError, new { Message = responseResultObj.Message });
        }

        private IActionResult ReturnCustomResponse(OperationOutcome outcome, string message, object result, Uri uri)
        {
            return outcome switch
            {
                OperationOutcome.Success => Ok(new { Message = message, Data = result }),
                OperationOutcome.Created => Created(uri, result),
                OperationOutcome.NotFound => NotFound(new { Message = message }),
                OperationOutcome.BadRequest => BadRequest(new { Message = message }),
                OperationOutcome.Unauthorized => Unauthorized(new { Message = message }),
                OperationOutcome.Forbidden => Forbid(),
                OperationOutcome.InternalError => StatusCode(StatusCodes.Status500InternalServerError, new { Message = message }),
                _ => StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Unknown error occurred." }),
            }; ;
        }
    }
}