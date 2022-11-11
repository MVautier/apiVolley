using ApiColomiersVolley.BLL.DMItem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMItem.Business.Interfaces
{
    public interface IBSItem
    {
        Task<IEnumerable<DtoItem>> GetListe();
        Task<IEnumerable<DtoItem>> GetTree();
        Task<DtoItem> AddOrUpdate(DtoItem item);
        Task<bool> Remove(int id);
    }
}
