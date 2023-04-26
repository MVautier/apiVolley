using ApiColomiersVolley.BLL.DMUser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMUser.Repositories
{
    public interface IDMUser
    {
        Task<IEnumerable<DtoUserRole>> GetListe();
    }
}
