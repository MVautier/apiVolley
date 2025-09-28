using ApiColomiersVolley.BLL.DMAdherent.Business;
using ApiColomiersVolley.BLL.DMAdherent.Models;
using ApiColomiersVolley.BLL.DMAdherent.Repositories;
using ApiColomiersVolley.BLL.DMParametres.Models;
using ApiColomiersVolley.BLL.DMParametres.Repositories;
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
    public class DPParametres : IDMParametresRepo
    {
        private ColomiersVolleyContext _db { get; set; }

        public DPParametres(ColomiersVolleyContext db)
        {
            _db = db;
        }

        private IQueryable<Parametres> GetAll()
        {
            return _db.Parametres;
        }

        public async Task<DtoParametres> Get()
        {
            return (await GetAll().FirstOrDefaultAsync()).ToDtoParametres();
        }

        public async Task<DtoParametres> AddOrUpdate(DtoParametres param)
        {
            if (param.IdParametre > 0)
            {
                Parametres p = await GetAll().FirstOrDefaultAsync(p => p.IdParametre == param.IdParametre);
                if (p != null)
                {
                    p = param.ToParametres(p);
                    await _db.SaveChangesAsync();
                    return param;
                }

                return null;
            }
            else
            {
                var newParam = await _db.AddAsync(param.ToParametresAdd());
                await _db.SaveChangesAsync();
                if (newParam != null)
                {
                    param.IdParametre = newParam.Entity.IdParametre;
                    return param;
                }

                return null;
            }
        }
    }
}
