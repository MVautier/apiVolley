using ApiColomiersVolley.BLL.DMArticle.Models;
using ApiColomiersVolley.BLL.DMArticle.Repositories;
using ApiColomiersVolley.DAL.Entities.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ApiColomiersVolley.DAL.DataProviders
{
    public class DPArticle : IDMArticleRepo
    {
        private ColomiersVolleyContext _db { get; set; }

        public DPArticle(ColomiersVolleyContext db)
        {
            _db = db;
        }

        private IQueryable<Entities.Article> GetAll()
        {
            return _db.Articles;
        }

        public async Task<List<DtoArticle>> Get()
        {
            return (await GetAll().ToListAsync()).ToDtoArticle();
        }
    }
}
