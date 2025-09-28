using ApiColomiersVolley.BLL.DMAdherent.Business.Interfaces;
using ApiColomiersVolley.BLL.DMAdherent.Models;
using ApiColomiersVolley.BLL.DMAdherent.Repositories;
using ApiColomiersVolley.BLL.DMParametres.Business;
using ApiColomiersVolley.BLL.DMParametres.Business.Interfaces;
using Microsoft.Extensions.Configuration;
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
        private readonly IDMAdherentRepo _adhRepo;
        private readonly IBSParametres _bsParametres;
        private readonly IConfiguration _config;

        public BSCategory(IDMCategoryRepo categRepo, IDMAdherentRepo adhRepo, IBSParametres bsParametres, IConfiguration config)
        {
            _categRepo = categRepo;
            _adhRepo = adhRepo;
            _bsParametres = bsParametres;
            _config = config;
        }

        public async  Task<IEnumerable<DtoCategory>> GetListe()
        {
            int year = DateTime.Now.Year;
            var ados = await _adhRepo.GetAdherentsByCategoryAndSeason(3, year);
            var categs = (await this._categRepo.GetCategories()).ToList();
            var param = await this._bsParametres.Get();
            if (param.AdoOpened)
            {
                if (ados.Any())
                {
                    if (ados.Count() > param.NbAdoMax)
                    {
                        categs.First(c => c.IdCategory == 3).Blocked = true;
                    }
                }
            } 
            else 
            {
                categs.First(c => c.IdCategory == 3).Blocked = true;
            }

            if (!param.CompetOpened)
            {
                categs.First(c => c.IdCategory == 1).Blocked = true;
            }

            if (!param.LoisirOpened)
            {
                categs.First(c => c.IdCategory == 2).Blocked = true;
            }

            return categs;
        }
    }
}
