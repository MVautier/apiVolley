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
        Task<IEnumerable<WebItem>> GetListe();
        Task<Tree> GetTree();
        Task<WebItem> AddOrUpdate(WebItem item);
        Task<bool> Remove(int id);
    }
}
