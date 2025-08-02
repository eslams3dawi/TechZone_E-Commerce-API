using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using TechZone.API.Middleware.CustomExceptions;
using TechZone.BLL.Wrappers;

namespace TechZone.API.Middleware
{
    public class ErrorHandlerException
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerException> _logger;

        public ErrorHandlerException(RequestDelegate next, ILogger<ErrorHandlerException> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                var result = new Result<string>();

                switch(error) //the value not the exception
                {
                    case KeyNotFoundException ex: //switch can match because it takes the exception as parameter
                    //case KeyNotFoundException: switch can not match because it match the value(error) with exception type → wrong
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        result = Result<string>.Failure(ex.Message, null, ActionCode.NotFound);
                        _logger.LogWarning("Bad Request {Message}", ex.Message);
                        break;

                    case NullReferenceException ex:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        result = Result<string>.Failure(ex.Message, null, ActionCode.NullReference);
                        _logger.LogError("Not Found: {Message}", ex.Message);

                        break;

                    case UnauthorizedAccessException ex:
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        result = Result<string>.Failure(ex.Message, null, ActionCode.Unauthorized);
                        _logger.LogWarning("Unauthorized {Message}", ex.Message);

                        break;
                    case BadRequestException ex:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        result = Result<string>.Failure(ex.Message, null, ActionCode.BadRequest);
                        _logger.LogWarning("Bad Request {Message}", ex.Message);
                        break;

                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        var message = error.Message;
                        var details = error.StackTrace;
                        var innerException = error.InnerException?.Message;

                        result = Result<string>.Failure(message, null, ActionCode.InternalServerError);

                        result.Errors = new List<string>
                        {
                            $"Inner Exception: {innerException ?? "No inner trace available"}",
                            $"Exception: {details ?? "No stack trace available."}" 
                        };
                        _logger.LogCritical(error, "Unhandled exception occurred");
                        break;
                }

                await response.WriteAsJsonAsync(result);
            }
        }
    }
}
