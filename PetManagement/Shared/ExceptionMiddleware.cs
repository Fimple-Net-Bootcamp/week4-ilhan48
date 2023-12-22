using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using static PetManagement.Shared.ExceptionMiddleware;

namespace PetManagement.Shared;

public class ExceptionMiddleware
{
    public class ExceptionResponse : Exception
    {
        public int StatusCode { get; set; }
        public List<string> Errors { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; }

        public ExceptionResponse(List<string> errors, int statusCode = StatusCodes.Status500InternalServerError, string message = "")
            : base(string.Join("; ", errors))
        {
            StatusCode = statusCode;
            Errors = errors;
            Message = message;
        }
    }

    public class ErrorMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context)
        {
            string message = "";
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            if (!context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";
                var response = new ExceptionResponse(new List<string> { message });
                var json = JsonConvert.SerializeObject(response);
                await context.Response.WriteAsync(json);
            }
        }
    }

}

public static class ErrorMiddlewareExtensions
{
    public static IApplicationBuilder UseErrorWrappingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorMiddleware>();
    }
}
