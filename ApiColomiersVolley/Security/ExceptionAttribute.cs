using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using ApiColomiersVolley.Extensions;
using ApiColomiersVolley.BLL.DMAuthentication.Exceptions;
using ApiColomiersVolley.BLL.Core.Tools.Interfaces;

namespace ApiColomiersVolley.Security
{
    public class ExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ExceptionAttribute>>();
            int status = (int)HttpStatusCode.Forbidden;
            var controllerName = "";
            var actionName = "";
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var exception = context.Exception;
            string error = null;

            if (controllerActionDescriptor != null)
            {
                controllerName = controllerActionDescriptor.ControllerName;
                actionName = controllerActionDescriptor.ActionName;
            }

            context.HttpContext.Request.Headers.TryGetValue("Referer", out Microsoft.Extensions.Primitives.StringValues referrer);
            var errorText = "Error detected in method " + actionName + " (controller: " + controllerName + ") for IP: " + context.HttpContext.GetRemoteIPAddress() + " referer : " + referrer;
            if (!(exception is AccessViolationException) && !(exception is InvalidPasswordException) && string.IsNullOrEmpty(error))
            {
                status = (int)HttpStatusCode.InternalServerError;
                var mail = context.HttpContext.RequestServices.GetRequiredService<IServiceSendMail>();
                mail.SendMailErreur(context.Exception, errorText);
            }

            if (status >= 500)
            {
                LogError(logger, context.Exception, errorText);
            }

            context.Result = new ContentResult
            {
                Content = !string.IsNullOrEmpty(error) ? error : exception.Message,
                ContentType = "text/plain",
                StatusCode = status
            };
        }

        private void LogError(ILogger<ExceptionAttribute> logger, Exception ex, string message)
        {
            if (ex != null)
            {
                logger.LogError(ex, message);
                if (ex.InnerException != null)
                {
                    LogError(logger, ex.InnerException, "Inner Exception");
                }
            }
        }
    }
}
