using ApiColomiersVolley.BLL.DMAdherent.Business;
using ApiColomiersVolley.BLL.DMAdherent.Models;
using ApiColomiersVolley.BLL.DMAdherent.Repositories;
using ApiColomiersVolley.BLL.DMParametres.Business.Interfaces;
using ApiColomiersVolley.BLL.DMParametres.Models;
using ApiColomiersVolley.BLL.DMParametres.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMParametres.Business
{
    public class BSParametres : IBSParametres
    {
        private readonly IDMParametresRepo _paramRepo;
        private readonly IConfiguration _config;

        public BSParametres(IDMParametresRepo paramRepo, IConfiguration config)
        {
            _paramRepo = paramRepo;
            _config = config;
        }

        public async Task<DtoParametres> Get()
        {
            return await _paramRepo.Get();
        }

        public async Task<DtoParametres> AddOrUpdate(DtoParametres param)
        {
            DtoParametres result = await _paramRepo.AddOrUpdate(param);
            return result;
        }
    }
}
