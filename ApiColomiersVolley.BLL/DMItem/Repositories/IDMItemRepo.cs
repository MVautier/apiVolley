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
        Task<List<WebItem>> GetListe();
        Task<List<WebItem>> GetByType(string type);
        Task<WebItem> AddOrUpdate(WebItem item);
        Task<bool> Remove(int id);
    }
}
