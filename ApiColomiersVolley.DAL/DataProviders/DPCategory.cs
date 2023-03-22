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
    public class DPCategory : IDMCategoryRepo
    {
        private ColomiersVolleyContext _db { get; set; }

        public DPCategory(ColomiersVolleyContext db)
        {
            _db = db;
        }

        private IQueryable<Category> GetAll()
        {
            return _db.Categories;
        }
        public async Task<IEnumerable<DtoCategory>> GetCategories()
        {
            return GetAll().ToDtoCategory();
        }
    }
}
