using ApiColomiersVolley.BLL.DMItem.Models;
using ApiColomiersVolley.BLL.DMItem.Repositories;
using ApiColomiersVolley.DAL.Entities;
using ApiColomiersVolley.DAL.Entities.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ApiColomiersVolley.DAL.DataProviders
{
    public class DPItem : IDMItemRepo
    {
        private ColomiersVolleyContext _db { get; set; }

        public DPItem(ColomiersVolleyContext db)
        {
            _db = db;
        }

        private IQueryable<Entities.Item> GetAll()
        {
            return _db.Items;
        }

        public async Task<List<DtoItem>> GetListe()
        {
            return (await GetAll().ToListAsync()).ToDtoItem();
        }

        public async Task<Item> GetById(int id)
        {
            return (await GetAll().FirstOrDefaultAsync(p => p.IdItem == id));
        }

        public async Task<List<DtoItem>> GetByType(string type)
        {
            return (await GetAll().Where(p => p.Type == type).ToListAsync()).ToDtoItem();
        }

        public async Task<DtoItem> AddOrUpdate(DtoItem item)
        {
            return item.IdItem != 0 ? await Update(item.ToItem()) : await Add(item.ToItem());
        }

        public async Task<bool> Remove(int id)
        {
            Item item = await GetById(id);
            if (item == null)
            {
                return false;
            }
            var result = _db.Items.Remove(item);
            await _db.SaveChangesAsync();
            return true;
        }

        private async Task<DtoItem> Add(Item item)
        {
            var result = await _db.Items.AddAsync(item);
            await _db.SaveChangesAsync();
            return result.Entity.ToDtoItem();
        }

        private async Task<DtoItem> Update(Item item)
        {
            var result = _db.Items.Update(item);
            await _db.SaveChangesAsync();
            return result.Entity.ToDtoItem();
        }
    }
}
