using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiColomiersVolley.Security
{
    public class AllowOriginFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                var origin = context.HttpContext.Request.Headers.Origin;
                //if (context.HttpContext.Request.Path == "/api/bank/redirection" && origin == "https://sogecommerce.societegenerale.eu")
                //{
                //    base.OnActionExecuting(context);
                //    return;
                //}
                //IAuthClientAppsRepo authClientAppsRepo = context.HttpContext.RequestServices.GetService<IAuthClientAppsRepo>();
                //if (!string.IsNullOrEmpty(origin))
                //{
                //    var clientAppSingleton = ClientAppSingleton.Instance;
                //    if (!clientAppSingleton.clientApps.Any())
                //    {
                //        clientAppSingleton.clientApps = authClientAppsRepo.GetClientApps().GetAwaiter().GetResult().ToList();
                //    }

                //    if (!IsAllowedOrigin(clientAppSingleton.clientApps, origin))
                //    {
                //        if (context.HttpContext.Response.Headers.ContainsKey("Access-Control-Allow-Origin"))
                //        {
                //            context.HttpContext.Response.Headers["Access-Control-Allow-Origin"] = "Invalid origin";
                //        }
                //        else
                //        {
                //            context.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "Invalid origin");
                //        }
                //        throw new UnauthorizedAccessException("Origin");
                //    }
                //}
            }
            catch (UnauthorizedAccessException ex)
            {
                context.Result = new ContentResult
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }
            catch (Exception ex)
            {
                context.Result = new ContentResult
                {
                    Content = ex.Message,
                    ContentType = "text/plain",
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
            base.OnActionExecuting(context);
        }
    }
}
