using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SnippetManagement.Api.Exceptions;

namespace SnippetManagement.Api.Middlewares;

public class HandleApiExceptionAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        var exceptionType = context.Exception.GetType();
        var result = new ObjectResult(new
        {
            context.Exception.Message, 
            context.Exception.Source,
            ExceptionType = context.Exception.GetType().FullName,
            context.Exception.StackTrace
        })
        {
            StatusCode = GetStatusCode(exceptionType),
        };

        context.Result = result;
    }

    private int? GetStatusCode(Type exceptionType)
    {
        switch (exceptionType)
        {
            case var value when value == typeof(BadRequestException):
                return StatusCodes.Status400BadRequest;
            case var value when value == typeof(NotFoundException):
                return StatusCodes.Status404NotFound;
            case var value when value == typeof(NotImplementedException):
                return StatusCodes.Status501NotImplemented;
            case var value when value == typeof(UnauthorizedAccessException):
                return StatusCodes.Status401Unauthorized;
            case var value when value == typeof(KeyNotFoundException):
                return StatusCodes.Status404NotFound;
            default:
                return StatusCodes.Status500InternalServerError;
        }
    }
}