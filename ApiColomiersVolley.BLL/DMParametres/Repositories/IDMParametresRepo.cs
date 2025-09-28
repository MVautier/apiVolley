using ApiColomiersVolley.BLL.DMParametres.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMParametres.Repositories
{
    public interface IDMParametresRepo
    {
        Task<DtoParametres> Get();
        Task<DtoParametres> AddOrUpdate(DtoParametres param);
    }
}
