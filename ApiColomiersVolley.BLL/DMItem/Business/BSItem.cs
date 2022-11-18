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

        public BSItem(IConfiguration config, IDMItemRepo itemRepo)
        {
            _config = config;
            _itemRepo = itemRepo;
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
            return new Tree()
            {
                pages = pages.Any() ? pages : new List<WebItem>(),
                posts = pages.Any() ? posts : new List<WebItem>(),
                comments = pages.Any() ? comments : new List<WebItem>()
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
