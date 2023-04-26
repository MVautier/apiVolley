using ApiColomiersVolley.BLL.DMAuthentication.Models;
using ApiColomiersVolley.BLL.DMAuthentication.Repositories;
using ApiColomiersVolley.BLL.DMUser.Models;
using ApiColomiersVolley.BLL.DMUser.Repositories;
using ApiColomiersVolley.DAL.Entities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.DAL.DataProviders
{
    public class DPUser : IDMUserRepo, IDMUser
    {
        private ColomiersVolleyContext _db { get; set; }

        public DPUser(ColomiersVolleyContext db)
        {
            _db = db;
        }

        private IQueryable<Entities.User> GetAll()
        {
            return _db.Users.Include(u => u.Role);
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
            return (await GetAll().FirstOrDefaultAsync(u => u.Mail.ToLower() == mail.ToLower())).ToDtoUser();
        }

        public async Task<DtoUser> Authenticate(string mail, string password)
        {
            return (await GetAll().FirstOrDefaultAsync(u => u.Mail.ToLower() == mail.ToLower() && u.Password == password)).ToDtoUser();
        }

        public async Task<IEnumerable<DtoUserRole>> GetListe()
        {
            return (await GetAll().ToListAsync()).ToDtoUserRole();
        }

        public async Task<UserInfo> GetConnectingUser(Login login)
        {
            return (await GetAll()
                .FirstOrDefaultAsync(c => c.Mail.ToLower() == login.Email.ToLower() && c.Password == login.Password && (!c.EndDate.HasValue || c.EndDate.Value > DateTime.Now)))
                .ToUserInfo();
        }

        public async Task<UserInfo> GetRefreshUser(int idUser)
        {
            return (await GetAll().FirstOrDefaultAsync(c => c.IdUser == idUser && (!c.EndDate.HasValue || c.EndDate.Value > DateTime.Now))).ToUserInfo();
        }
    }
}
