using ApiColomiersVolley.BLL.DMAuthentication.Business;
using ApiColomiersVolley.BLL.DMAuthentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMAuthentication.Repositories
{
    public interface IDMConnexionRepo
    {
        Task<DtoConnexion> GetLastUserConnexion(int idUser, string ip);
        Task<DtoConnexion> Add(DtoConnexion conn);
        Task<DtoConnexion> IncrementRefresh(int idConnexion, DateTime endDate);
        Task<DtoConnexion> Get(int idConnexion);
        Task Delete(int idConnexion);
    }
}
