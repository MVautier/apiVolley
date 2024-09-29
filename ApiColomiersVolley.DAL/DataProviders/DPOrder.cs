using ApiColomiersVolley.BLL.DMAdherent.Models;
using ApiColomiersVolley.BLL.DMAdherent.Repositories;
using ApiColomiersVolley.BLL.DMArticle.Models;
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
    public class DPOrder : IDMOrderRepo
    {
        private ColomiersVolleyContext _db { get; set; }

        public DPOrder(ColomiersVolleyContext db)
        {
            _db = db;
        }

        private IQueryable<Entities.Order> GetAll()
        {
            return _db.Orders;
        }

        public async Task<List<DtoOrder>> Get()
        {
            return (await GetAll().ToListAsync()).ToDtoOrder();
        }

        public async Task<DtoOrder> GetById(int id)
        {
            return (await GetAll().FirstOrDefaultAsync(o => o.IdOrder == id)).ToDtoOrder();
        }

        public async Task<List<DtoOrder>> GetByAdherent(int idAdherent)
        {
            return (await GetAll().Where(o => o.IdAdherent == idAdherent).ToListAsync()).ToDtoOrder();
        }

        public async Task<List<DtoOrder>> GetByDateRange(DateTime start, DateTime? end)
        {
            if (end == null)
            {
                end = DateTime.Now.AddDays(1);
            }
            return (await GetAll().Where(o => o.Date >= start && o.Date < end).ToListAsync()).ToDtoOrder();
        }

        public async Task<DtoOrder> AddOrUpdate(DtoOrder order)
        {
            Order o = null;
            if (order.Id > 0)
            {
                o = await GetAll().FirstOrDefaultAsync(a => a.IdOrder == order.Id);
                if (o != null)
                {
                    o = order.ToOrder(o);
                    await _db.SaveChangesAsync();
                    return order;
                }

                return null;
            }
            else
            {
                var newOrder = await _db.AddAsync(order.ToOrderAdd());
                await _db.SaveChangesAsync();
                if (newOrder != null)
                {
                    order.Id = newOrder.Entity.IdOrder;
                    return order;
                }

                return null;
            }
        }
    }
}
