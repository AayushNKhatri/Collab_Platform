using Serilog;
using System.Text;

namespace Collab_Platform.PresentationLayer.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        public LoggingMiddleware(RequestDelegate next) 
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();

            string body = "";
            if (context.Request.ContentLength > 0) {
                var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen:true);
                body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            Log.Information("Incomming Reqeust: {method} {path} Body: {body}", 
                context.Request.Method,
                context.Request.Path,
                body
                );
            var originalResponseBody = context.Response.Body;
            var newResponseBody = new MemoryStream();
            context.Response.Body = newResponseBody;
            await _next(context);
            newResponseBody.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(newResponseBody).ReadToEndAsync();
            newResponseBody.Seek(0, SeekOrigin.Begin);

            Log.Information("Outgoing Response: {statusCode} Body: {body}",
                            context.Response.StatusCode,
                            responseText);

            await newResponseBody.CopyToAsync(originalResponseBody);
            context.Response.Body = originalResponseBody;
        }
    }
}
