namespace ApiColomiersVolley.Middlewares
{
    public class CorsMiddleware
    {
        private readonly RequestDelegate _next;

        public CorsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            httpContext.Response.Headers.Add("Access-Control-Allow-Origin", httpContext.Request.Headers.GetCommaSeparatedValues("Origin").FirstOrDefault());
            string[] headersAllow = httpContext.Request.Headers.GetCommaSeparatedValues("Access-Control-Request-Headers");
            if (headersAllow?.Any() ?? false)
            {
                httpContext.Response.Headers.Add("Access-Control-Allow-Headers", string.Join(',', headersAllow));
            }

            httpContext.Response.Headers.Add("Access-Control-Allow-Methods", httpContext.Request.Headers.GetCommaSeparatedValues("Access-Control-Request-Method").FirstOrDefault());
            httpContext.Response.Headers.Add("Access-Control-Allow-Credentials", "true");

            if (httpContext.Request.Method == "OPTIONS")
            {
                httpContext.Response.Body.FlushAsync();
            }

            return _next(httpContext);
        }
    }

    public static class CorsMiddlewareExtensions
    {
        public static IApplicationBuilder UseCorsMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CorsMiddleware>();
        }
    }
}
