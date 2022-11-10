using ApiColomiersVolley.BLL.DMAuthentication.Models;
using ApiColomiersVolley.Security;
using ApiColomiersVolley.Security.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiColomiersVolley
{
    [Exception]
    public class ColomiersVolleyController : ControllerBase
    {
        private UserInfo _user { get; set; }

        public UserInfo _userAuth
        {
            get
            {
                if (_user == null)
                {
                    _user = HttpContext.RequestServices.GetRequiredService<IOAuthHelper>().CreateUserInfosFromSecurityToken();
                }

                return _user;
            }
        }
    }
}
