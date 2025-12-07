using Serilog;
using System.Net;
using System.Text.Json;
using static Collab_Platform.PresentationLayer.Middleware.ExecptionClass;

namespace Collab_Platform.PresentationLayer.Middleware
{
    public class GlobalException
    {
        private readonly RequestDelegate _next;

        public GlobalException(RequestDelegate next) {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context) 
        {
            try {
                await _next(context);
            }
            catch (Exception e) {
                Log.Error(e,

                    "Unhandled exception occurred. Path: {Path}, Method: {Method}, TraceId: {TraceId}",
                    context.Request.Path,
                    context.Request.Method,
                    context.TraceIdentifier);

                await HandleExceptionAsync(context, e);
            }
        }
        private static async Task HandleExceptionAsync(HttpContext context, Exception e)
        {
            var status = e switch
            {
                ArgumentException => HttpStatusCode.BadRequest,
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                KeyNotFoundException => HttpStatusCode.NotFound,
                InvalidOperationException => HttpStatusCode.Conflict,
                InvalidRoleException => HttpStatusCode.Forbidden,
                _ => HttpStatusCode.InternalServerError,
            };

            var response = new
            {
                success = false,
                messege = e.Message,
                error = e.GetType().Name,
                traceID = context.TraceIdentifier,
                path = context.Request.Path,
            };
            
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            var option = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase , WriteIndented = true};
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, option));
        }

    }
}
