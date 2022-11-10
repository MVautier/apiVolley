using ApiColomiersVolley.BLL.DMArticle.Models;
using ApiColomiersVolley.BLL.DMAuthentication.Models;
using ApiColomiersVolley.BLL.DMAuthentication.Repositories;
using ApiColomiersVolley.DAL.Entities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.DAL.DataProviders
{
    public class DPUser : IDMUserRepo
    {
        private ColomiersVolleyContext _db { get; set; }

        public DPUser(ColomiersVolleyContext db)
        {
            _db = db;
        }

        private IQueryable<Entities.User> GetAll()
        {
            return _db.Users;
        }

        public async Task<List<DtoUser>> Get()
        {
            return (await GetAll().ToListAsync()).ToDtoUser();
        }

        public async Task<DtoUser> GetById(int id)
        {
            return (await GetAll().FirstOrDefaultAsync(u => u.IdUser == id)).ToDtoUser();
        }

        public async Task<DtoUser> GetByMail(string mail)
        {
            return (await GetAll().FirstOrDefaultAsync(u => u.Mail == mail)).ToDtoUser();
        }

        public async Task<DtoUser> Authenticate(string mail, string password)
        {
            return (await GetAll().FirstOrDefaultAsync(u => u.Mail == mail && u.Password == password)).ToDtoUser();
        }
    }
}
