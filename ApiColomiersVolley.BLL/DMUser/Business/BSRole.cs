using ApiColomiersVolley.BLL.DMItem.Repositories;
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
    public class BSRole : IBSRole
    {
        private readonly IConfiguration _config;
        private readonly IDMRoleRepo _roleRepo;

        public BSRole(IConfiguration config, IDMRoleRepo roleRepo)
        {
            _config = config;
            _roleRepo = roleRepo;
        }

        public async Task<IEnumerable<DtoRole>> GetRoles()
        {
            return await _roleRepo.GetRoles();
        }
    }
}
