using ApiColomiersVolley.BLL.DMAuthentication.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMAuthentication.Business.Interfaces
{
    public interface IBSAuthentication
    {
        Task<UserToken> LogInUser(Login login, string ip);
        Task<UserToken> RefreshUser(Refresh refresh, string ip);
        Task LogOutUser(string refreshId);
        Task<bool> CheckUserEmail(UserEmail userEmail);
    }
}
