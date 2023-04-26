using ApiColomiersVolley.BLL.DMAdherent.Models;
using ApiColomiersVolley.BLL.DMUser.Models;
using ApiColomiersVolley.BLL.DMUser.Repositories;
using ApiColomiersVolley.DAL.Entities;
using ApiColomiersVolley.DAL.Entities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.DAL.DataProviders
{
    public class DPRole : IDMRoleRepo
    {
        private ColomiersVolleyContext _db { get; set; }

        public DPRole(ColomiersVolleyContext db)
        {
            _db = db;
        }

        private IQueryable<Role> GetAll()
        {
            return _db.Roles;
        }

        public async Task<IEnumerable<DtoRole>> GetRoles()
        {
            return GetAll().ToDtoRole();
        }
    }
}
