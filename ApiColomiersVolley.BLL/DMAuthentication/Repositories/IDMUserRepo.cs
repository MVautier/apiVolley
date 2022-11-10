using ApiColomiersVolley.BLL.DMAuthentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMAuthentication.Repositories
{
    public interface IDMUserRepo
    {
        Task<DtoUser> GetById(int id);
        Task<DtoUser> GetByMail(string mail);
        Task<DtoUser> Authenticate(string mail, string password);
    }
}
