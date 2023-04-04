using ApiColomiersVolley.BLL.DMAuthentication.Models;
using ApiColomiersVolley.BLL.DMAuthentication.Repositories;
using ApiColomiersVolley.BLL.DMItem.Models;
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
    public class DPConnexion : IDMConnexionRepo
    {
        private ColomiersVolleyContext _db { get; set; }

        public DPConnexion(ColomiersVolleyContext db)
        {
            _db = db;
        }

        private IQueryable<Entities.Connexion> GetAll()
        {
            return _db.Connexions;
        }

        public async Task<DtoConnexion> GetLastUserConnexion(int idUser, string ip)
        {
            return (await GetAll().Where(c => c.IdUser == idUser && c.Ip == ip).FirstOrDefaultAsync()).ToDtoConnexion();
        }

        public async Task<DtoConnexion> Add(DtoConnexion conn)
        {
            var result = await _db.Connexions.AddAsync(conn.ToConnexion());
            await _db.SaveChangesAsync();
            return result.Entity.ToDtoConnexion();
        }

        public async Task<DtoConnexion> IncrementRefresh(int idConnexion, DateTime endDate)
        {
            var cnx = await _db.Connexions.FirstOrDefaultAsync(c => c.IdConnexion == idConnexion);
            if (cnx != null)
            {
                cnx.EndDate = endDate;
                cnx.RefreshCount++;
                cnx.LastRefresh = DateTime.Now;
                await _db.SaveChangesAsync();
            }

            return cnx.ToDtoConnexion();
        }

        public async Task<DtoConnexion> Get(int idConnexion)
        {
            return (await _db.Connexions.FirstOrDefaultAsync(c => c.IdConnexion == idConnexion)).ToDtoConnexion();
        }

        public async Task Delete(int idConnexion)
        {
            var cnx = await _db.Connexions.FirstOrDefaultAsync(c => c.IdConnexion == idConnexion);
            if (cnx != null)
            {
                cnx.EndDate = DateTime.Now;
                await _db.SaveChangesAsync();
            }
        }
    }
}
