using ApiColomiersVolley.BLL.DMItem.Models;
using ApiColomiersVolley.BLL.DMItem.Repositories;
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
    public class DPArticlePage : IDMArticlePageRepo
    {
        private ColomiersVolleyContext _db { get; set; }

        public DPArticlePage(ColomiersVolleyContext db)
        {
            _db = db;
        }

        public async Task<DtoArticlePage> AddOrUpdate(DtoArticlePage item)
        {
            return item.IdArticlePage != 0 ? await Update(item.ToArticlePage()) : await Add(item.ToArticlePage());
        }

        public async Task<List<DtoArticlePage>> GetByIdArticle(int idArticle)
        {
            return (await GetAll().Where(p => p.IdArticle == idArticle).ToListAsync()).ToDtoArticlePage();
        }

        public async Task<bool> Remove(int id)
        {
            ArticlePage item = await GetById(id);
            if (item == null)
            {
                return false;
            }
            var result = _db.ArticlePages.Remove(item);
            await _db.SaveChangesAsync();
            return true;
        }

        private IQueryable<ArticlePage> GetAll()
        {
            return _db.ArticlePages;
        }

        private async Task<ArticlePage> GetById(int id)
        {
            return (await GetAll().FirstOrDefaultAsync(p => p.IdArticlePage == id));
        }

        private async Task<DtoArticlePage> Add(ArticlePage item)
        {
            var result = await _db.ArticlePages.AddAsync(item);
            await _db.SaveChangesAsync();
            return result.Entity.ToDtoArticlePage();
        }

        private async Task<DtoArticlePage> Update(ArticlePage item)
        {
            var result = _db.ArticlePages.Update(item);
            await _db.SaveChangesAsync();
            return result.Entity.ToDtoArticlePage();
        }
    }
}
