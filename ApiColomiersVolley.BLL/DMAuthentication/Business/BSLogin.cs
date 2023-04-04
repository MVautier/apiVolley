using ApiColomiersVolley.BLL.DMAuthentication.Business.Interfaces;
using ApiColomiersVolley.BLL.DMAuthentication.Models;
using ApiColomiersVolley.BLL.DMAuthentication.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMAuthentication.Business
{
    public class BSLogin : IBSLogin
    {
        private readonly IConfiguration _config;
        private readonly IDMUserRepo _userRepo;

        public BSLogin(IConfiguration config, IDMUserRepo userRepo)
        {
            _config = config;
            _userRepo = userRepo;
        }

        public Task<DtoUser> Authenticate(string mail, string password)
        {
            return _userRepo.Authenticate(mail, password);
        }
    }
}
