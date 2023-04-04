using ApiColomiersVolley.BLL.DMAuthentication.Models;
using ApiColomiersVolley.BLL.DMAuthentication.Repositories;
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
    public class DPToken : IDMTokenRepo
    {
        private ColomiersVolleyContext _db { get; set; }

        public DPToken(ColomiersVolleyContext db)
        {
            _db = db;
        }

        private IQueryable<Entities.Token> GetAll()
        {
            return _db.Tokens;
        }

        public async Task<DtoToken> Add(DtoToken token)
        {
            var result = await _db.Tokens.AddAsync(token.ToToken());
            await _db.SaveChangesAsync();
            return result.Entity.ToDtoToken();
        }

        public async Task<DtoToken> Get(string tokenKey)
        {
            return (await _db.Tokens.FirstOrDefaultAsync(t => t.Key == tokenKey && t.EndDate > DateTime.Now)).ToDtoToken();
        }

        public async Task<bool> Exists(string tokenKey)
        {
            return await _db.Tokens.AnyAsync(t => t.Key == tokenKey && t.EndDate > DateTime.Now);
        }

        public async Task Delete(string tokenKey)
        {
            var token = await _db.Tokens.FirstOrDefaultAsync(c => c.Key == tokenKey);
            if (token != null)
            {
                token.EndDate = DateTime.Now;
                await _db.SaveChangesAsync();
            }
        }

        public async Task DeleteByConnexion(int idConnexion)
        {
            var token = await _db.Tokens.Where(c => c.IdConnexion == idConnexion).ToListAsync();
            foreach (var t in token)
            {
                t.EndDate = DateTime.Now;
            }

            await _db.SaveChangesAsync();
        }
    }
}
