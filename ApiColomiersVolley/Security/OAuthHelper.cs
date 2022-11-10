using ApiColomiersVolley.BLL.Core.Tools;
using ApiColomiersVolley.BLL.Core.Tools.Interfaces;
using ApiColomiersVolley.BLL.DMAuthentication.Models;
using ApiColomiersVolley.Security.Interfaces;
using Microsoft.Extensions.Primitives;

namespace ApiColomiersVolley.Security
{
    public class OAuthHelper : IOAuthHelper
    {
        private readonly IConfiguration _config;
        private readonly IServiceSendMail _mail;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IJWTFactory _jwtFactory;

        public OAuthHelper(IJWTFactory jwtFactory, IConfiguration config, IServiceSendMail mail, IHttpContextAccessor httpContext)
        {
            _jwtFactory = jwtFactory;
            _config = config;
            _mail = mail;
            _httpContext = httpContext;
        }

        //Check la validité du token
        public UserInfo CreateUserInfosFromSecurityToken()
        {

            string tkn = GetToken();
            bool isJwtToken = tkn.Split('.').Length == 3;
            UserInfo user = null;
            if (!string.IsNullOrWhiteSpace(tkn) && isJwtToken)
            {
                user = _jwtFactory.CheckAndExtract<UserInfo>(tkn);

                if (DateTime.Now > user.ExpireDate)
                {
                    return null;
                }

            }

            return user;
        }

        public string GetClientIpAddress()
        {
            return _httpContext.HttpContext.Connection.RemoteIpAddress.ToString();
        }

        public string GetToken()
        {
            string token = string.Empty;
            try
            {
                if (_httpContext.HttpContext != null)
                {
                    if (_httpContext.HttpContext.Request.Headers.TryGetValue("authorization", out StringValues auth) || _httpContext.HttpContext.Request.Headers.TryGetValue("Authorization", out auth))
                    {
                        token = auth.ToString().Split(' ')[1];
                    }
                }
            }
            catch (Exception ex)
            {
                _mail.SendMailErreur(ex, "Erreur lors de l'obtention du token");
            }

            return token;
        }
    }
}
