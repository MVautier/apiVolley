using ApiColomiersVolley.BLL.DMAdherent.Business.Interfaces;
using ApiColomiersVolley.BLL.DMAdherent.Models;
using ApiColomiersVolley.BLL.DMAdherent.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMAdherent.Business
{
    public class BSCategory : IBSCategory
    {
        private readonly IDMCategoryRepo _categRepo;

        public BSCategory(IDMCategoryRepo categRepo)
        {
           _categRepo = categRepo;
        }

        public async  Task<IEnumerable<DtoCategory>> GetListe()
        {
            return await this._categRepo.GetCategories();
        }
    }
}
