using DoohClick.Model.Shared.Enum.ResponseEnum;
using DoohClick.Model.Shared.Exceptions;
using DoohClick.Model.Shared.Response;

namespace DoohClick.API.Middleware
{
    public class GlobalExpectionHandler
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;

        public GlobalExpectionHandler(RequestDelegate next, IHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        public async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            if(ex is AppException appEx)
            {
                context.Response.StatusCode = appEx.StatusCode;
                await context.Response.WriteAsJsonAsync(ApiResponse.Failure(appEx.Message));
                return;
            }
           
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            string message = _env.IsDevelopment() ? ex.Message : "An unexpected error occurred.";
            await context.Response.WriteAsJsonAsync(ApiResponse.Failure(message, ResponseStatusEnum.ServerError.ToString()));
            }
    }
}
