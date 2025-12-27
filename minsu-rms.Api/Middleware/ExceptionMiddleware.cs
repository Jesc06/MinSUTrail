using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace RecordManagementSystem.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
            catch(ValidationException ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 400;
                var traceId = context.TraceIdentifier;

                var errorResponse = new
                {
                    statusCode = 500,
                    message = ex.Message,
                    traceId = traceId,
                    StackTrace = ex.StackTrace
                };

                await context.Response.WriteAsJsonAsync(errorResponse);
            }
            catch(Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;
              
                var traceId = context.TraceIdentifier;

                var errorResponse = new
                {
                    statusCode = 500,
                    message = "Internal server error.",
                    traceId = traceId,
                    StackTrace = ex.StackTrace
                };

                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }



    }
}
