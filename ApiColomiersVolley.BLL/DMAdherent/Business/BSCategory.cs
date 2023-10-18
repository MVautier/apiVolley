using ApiColomiersVolley.BLL.DMAdherent.Business.Interfaces;
using ApiColomiersVolley.BLL.DMAdherent.Models;
using ApiColomiersVolley.BLL.DMAdherent.Repositories;
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
        private readonly IConfiguration _config;

        public BSCategory(IDMCategoryRepo categRepo, IDMAdherentRepo adhRepo, IConfiguration config)
        {
            _categRepo = categRepo;
            _adhRepo = adhRepo;
            _config = config;
        }

        public async  Task<IEnumerable<DtoCategory>> GetListe()
        {
            int year = DateTime.Now.Year;
            var ados = await _adhRepo.GetAdherentsByCategoryAndSeason(3, year);
            var categs = (await this._categRepo.GetCategories()).ToList();
            if (ados.Any())
            {
                int nbMax = _config.GetValue<int>("nbAdoMax");
                if (ados.Count() > nbMax)
                {
                    categs.First(c => c.IdCategory == 3).Blocked = true;
                }

                if (!_config.GetValue<bool>("loisir_opened"))
                {
                    categs.First(c => c.IdCategory == 2).Blocked = true;
                }
                
            }
            return categs;
        }
    }
}
