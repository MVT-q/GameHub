using GameHubAPI.Exceptions;

namespace GameHubAPI.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                var statusCode = e switch
                {
                    UserAlreadyExistsException => StatusCodes.Status409Conflict,

                    _ => StatusCodes.Status500InternalServerError
                };

                context.Response.StatusCode = statusCode;

                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(new
                {
                    message = e.Message
                });
            }
        }
    }
}
