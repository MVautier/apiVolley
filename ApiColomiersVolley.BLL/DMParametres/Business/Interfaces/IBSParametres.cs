using ApiColomiersVolley.BLL.DMParametres.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMParametres.Business.Interfaces
{
    public interface IBSParametres
    {
        Task<DtoParametres> Get();
        Task<DtoParametres> AddOrUpdate(DtoParametres param);
    }
}
