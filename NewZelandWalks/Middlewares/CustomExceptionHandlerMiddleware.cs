using System.Net;

namespace NZWalks.API.Middlewares
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly ILogger<CustomExceptionHandlerMiddleware> logger;
        private readonly RequestDelegate next;

        public CustomExceptionHandlerMiddleware(ILogger<CustomExceptionHandlerMiddleware> logger, RequestDelegate next)
        {
            this.logger = logger;
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                var errorId = Guid.NewGuid();

                //Log this Exception(here we are logging the actual exception that occured in the code) 
                logger.LogError(ex, ex.Message);

                /* Return a custom error response(we do not want to send the actual error to the client 
                 * so for any exception that may occur during execution of any line of code,
                 * the following error response will be sent back) */
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                //error object will be the payload of the response 
                var error = new
                {
                    Id = errorId,
                    ErrorMessage = "Something went wrong, We are looking into it"
                };
                //
                await httpContext.Response.WriteAsJsonAsync(error);
            }
        }


    }
}
