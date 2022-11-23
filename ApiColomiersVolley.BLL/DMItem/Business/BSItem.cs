using ApiColomiersVolley.BLL.DMItem.Business.Interfaces;
using ApiColomiersVolley.BLL.DMItem.Models;
using ApiColomiersVolley.BLL.DMItem.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMItem.Business
{
    public class BSItem : IBSItem
    {
        private readonly IConfiguration _config;
        private readonly IDMItemRepo _itemRepo;
        private readonly IDMArticlePageRepo _articlePageRepo;

        public BSItem(IConfiguration config, IDMItemRepo itemRepo, IDMArticlePageRepo articlePageRepo)
        {
            _config = config;
            _itemRepo = itemRepo;
            _articlePageRepo = articlePageRepo;
        }

        public async Task<IEnumerable<WebItem>> GetListe()
        {
            return await _itemRepo.GetListe();
        }

        public async Task<Tree> GetTree()
        {
            var pages = await _itemRepo.GetByType("page");
            var posts = await _itemRepo.GetByType("post");
            var comments = await _itemRepo.GetByType("comment");

            if (posts.Any())
            {
                foreach(var post in posts)
                {
                    var liaisons = await _articlePageRepo.GetByIdArticle(post.IdItem);
                    post.IdPages = liaisons.Any() ? liaisons.Select(l => l.IdPage).ToList() : null;
                }
            }

            return new Tree()
            {
                pages = pages.Any() ? pages.OrderBy(p => p.Order).ToList() : new List<WebItem>(),
                posts = posts.Any() ? posts.OrderBy(p => p.Order).ToList() : new List<WebItem>(),
                comments = comments.Any() ? comments.OrderBy(p => p.Order).ToList() : new List<WebItem>()
            };
        }

        public async Task<WebItem> AddOrUpdate(WebItem item)
        {
            return await _itemRepo.AddOrUpdate(item);
        }

        public async Task<bool> Remove(int id)
        {
            return await _itemRepo.Remove(id);
        }
    }
}
