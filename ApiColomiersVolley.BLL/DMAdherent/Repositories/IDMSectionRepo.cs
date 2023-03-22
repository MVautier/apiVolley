using ApiColomiersVolley.BLL.DMAdherent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMAdherent.Repositories
{
    public interface IDMSectionRepo
    {
        Task<IEnumerable<DtoSection>> GetSections();
    }
}
