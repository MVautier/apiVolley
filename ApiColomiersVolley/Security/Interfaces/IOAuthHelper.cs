using ApiColomiersVolley.BLL.DMAuthentication.Models;

namespace ApiColomiersVolley.Security.Interfaces
{
    public interface IOAuthHelper
    {
        /// <summary>
        /// Obtient les informations du token de connexion
        /// </summary>
        /// <returns></returns>
        UserInfo CreateUserInfosFromSecurityToken();
    }
}
