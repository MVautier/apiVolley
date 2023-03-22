using ApiColomiersVolley.BLL.DMAdherent.Models;
using ApiColomiersVolley.BLL.DMAdherent.Repositories;
using ApiColomiersVolley.DAL.Entities;
using ApiColomiersVolley.DAL.Entities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.DAL.DataProviders
{
    public class DPSection : IDMSectionRepo
    {
        private ColomiersVolleyContext _db { get; set; }

        public DPSection(ColomiersVolleyContext db)
        {
            _db = db;
        }

        private IQueryable<Section> GetAll()
        {
            return _db.Sections;
        }

        public async Task<IEnumerable<DtoSection>> GetSections()
        {
            return GetAll().ToDtoSection();
        }
    }
}
