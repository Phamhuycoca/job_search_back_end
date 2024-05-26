using job_search_be.Api.Infrastructure.Middleware;

namespace job_search_be.Api.Infrastructure.Extensions
{
    public static class ExceptionMiddlewareExtension
    {
        public static void UseCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
