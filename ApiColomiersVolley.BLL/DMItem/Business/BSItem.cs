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

        public async Task<IEnumerable<DtoItem>> GetListe()
        {
            return await _itemRepo.GetListe();
        }

        public async Task<IEnumerable<DtoItem>> GetTree()
        {
            return await _itemRepo.GetByType("page");
        }

        public async Task<DtoItem> AddOrUpdate(DtoItem item)
        {
            return await _itemRepo.AddOrUpdate(item);
        }

        public async Task<bool> Remove(int id)
        {
            return await _itemRepo.Remove(id);
        }
    }
}
