using ApiColomiersVolley.BLL.Core.Models.Generic;
using ApiColomiersVolley.BLL.DMAdherent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMAdherent.Business.Interfaces
{
    public interface IBSAdherent
    {
        Task<IEnumerable<DtoAdherent>> GetListe();
        Task<PagedList<DtoAdherent>> GetPagedListe(AdherentFilter filter, Sorting sort, Pagination pager);
        Task<IEnumerable<DtoAdherent>> SearchAdherents(string name, string cp);
        Task<DtoAdherent> AddOrUpdate(DtoAdherent adherent);
    }
}
