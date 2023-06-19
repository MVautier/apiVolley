using ApiColomiersVolley.BLL.DMAdherent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMAdherent.Repositories
{
    public interface IDMOrderRepo
    {
        Task<List<DtoOrder>> Get();
        Task<DtoOrder> GetById(int id);
        Task<List<DtoOrder>> GetByAdherent(int idAdherent);
        Task<DtoOrder> AddOrUpdate(DtoOrder order);
    }
}
