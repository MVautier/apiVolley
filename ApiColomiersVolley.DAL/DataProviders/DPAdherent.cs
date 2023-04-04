using ApiColomiersVolley.BLL.DMAdherent.Models;
using ApiColomiersVolley.BLL.DMAdherent.Repositories;
using ApiColomiersVolley.DAL.Entities;
using ApiColomiersVolley.DAL.Entities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.DAL.DataProviders
{
    public class DPAdherent : IDMAdherentRepo
    {
        private ColomiersVolleyContext _db { get; set; }

        public DPAdherent(ColomiersVolleyContext db)
        {
            _db = db;
        }

        private IQueryable<Adherent> GetAll()
        {
            return _db.Adherents
                .Include(a => a.Section)
                .Include(a => a.Category);
        }

        public async Task<IEnumerable<DtoAdherent>> GetAdherents()
        {
            return (await GetAll().ToListAsync()).ToDtoAdherent();
        }

        public async Task<IEnumerable<DtoAdherent>> SearchAdherents(string name, string cp)
        {
            return (await GetAll().Where(a => a.LastName.ToLower() == name.ToLower() && a.PostalCode == cp).ToListAsync()).ToDtoAdherent();
        }
    }
}
