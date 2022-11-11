using ApiColomiersVolley.BLL.DMItem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMItem.Repositories
{
    public interface IDMItemRepo
    {
        Task<List<DtoItem>> GetListe();
        Task<List<DtoItem>> GetByType(string type);
        Task<DtoItem> AddOrUpdate(DtoItem item);
        Task<bool> Remove(int id);
    }
}
