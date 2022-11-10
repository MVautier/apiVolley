using ApiColomiersVolley.BLL.DMAuthentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMAuthentication.Business.Interfaces
{
    public interface IBSLogin
    {
        Task<DtoUser> Authenticate(string mail, string password);
    }
}
