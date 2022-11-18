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

        public async Task<List<WebItem>> GetListe()
        {
            var users = await _db.Users.ToListAsync();
            return (await GetAll().ToListAsync()).ToWebItem(users);
        }

        public async Task<Item> GetById(int id)
        {
            return (await GetAll().FirstOrDefaultAsync(p => p.IdItem == id));
        }

        public async Task<List<WebItem>> GetByType(string type)
        {
            var users = await _db.Users.ToListAsync();
            return (await GetAll().Where(p => p.Type == type).ToListAsync()).ToWebItem(users);
        }

        public async Task<WebItem> AddOrUpdate(WebItem item)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.IdUser == item.IdAuthor);
            return user != null ? (item.IdItem != 0 ? await Update(item.ToItem(), user) : await Add(item.ToItem(), user)) : null;
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

        private async Task<WebItem> Add(Item item, User user)
        {
            var result = await _db.Items.AddAsync(item);
            await _db.SaveChangesAsync();
            return result.Entity.ToWebItem(new List<User> { user });
        }

        private async Task<WebItem> Update(Item item, User user)
        {
            var result = _db.Items.Update(item);
            await _db.SaveChangesAsync();
            return result.Entity.ToWebItem(new List<User> { user });
        }
    }
}
