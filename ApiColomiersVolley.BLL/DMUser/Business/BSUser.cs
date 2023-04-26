using ApiColomiersVolley.BLL.DMUser.Business.Interfaces;
using ApiColomiersVolley.BLL.DMUser.Models;
using ApiColomiersVolley.BLL.DMUser.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMUser.Business
{
    public  class BSUser : IBSUser
    {
        private readonly IConfiguration _config;
        private readonly IDMUser _userRepo;

        public BSUser(IConfiguration config, IDMUser userRepo)
        {
            _config = config;
            _userRepo = userRepo;
        }

        public async Task<IEnumerable<DtoUserRole>> GetListe()
        {
            return await _userRepo.GetListe();
        }
    }
}
