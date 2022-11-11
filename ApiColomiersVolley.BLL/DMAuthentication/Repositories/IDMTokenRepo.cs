using ApiColomiersVolley.BLL.DMAuthentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMAuthentication.Repositories
{
    public interface IDMTokenRepo
    {
        Task<DtoToken> Add(DtoToken token);
        Task<DtoToken> Get(string tokenKey);
        Task<bool> Exists(string tokenKey);
        Task Delete(string tokenKey);
        Task DeleteByConnexion(int idConnexion);
    }
}
