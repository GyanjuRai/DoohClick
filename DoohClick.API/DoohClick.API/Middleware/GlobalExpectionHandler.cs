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

            await context.Response.WriteAsync("");
        }
    }
}
