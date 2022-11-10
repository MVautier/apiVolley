using ApiColomiersVolley.BLL.DMAuthentication.Business.Interfaces;
using ApiColomiersVolley.BLL.DMAuthentication.Exceptions;
using ApiColomiersVolley.BLL.DMAuthentication.Models;
using ApiColomiersVolley.BLL.DMAuthentication.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMAuthentication.Business
{
    public class BSAuthentication : IBSAuthentication
    {
        private readonly IConfiguration _config;
        private readonly IDMUserRepo _userRepo;

        public BSAuthentication(IConfiguration config, IDMUserRepo userRepo)
        {
            _config = config;
            _userRepo = userRepo;
        }

        public Task<AdminToken> LogInAdmin(Login login, string ip)
        {
            throw new NotImplementedException();
        }

        public Task<UserToken> LogInUser(Login login, string ip, int? idUserInfoInlog)
        {
            throw new NotImplementedException();
        }

        public Task LogOutUser(string refreshId)
        {
            throw new NotImplementedException();
        }

        public Task<AdminToken> RefreshAdmin(Refresh refresh, string ip)
        {
            throw new NotImplementedException();
        }

        public Task<UserToken> RefreshUser(Refresh refresh, string ip)
        {
            throw new NotImplementedException();
        }

        public Task ValidateClient(string login, string clientID, string origin, string referer, string ip, HttpResponse response)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CheckUserEmail(UserEmail userEmail)
        {
            if (string.IsNullOrEmpty(userEmail.Email))
            {
                throw new InvalidMailException();
            }
            var user = await _userRepo.GetByMail(userEmail.Email);
            return user == null;
            if (user != null)
            {
                throw new ExistingMailException("Un utilisateur est déjà enregistré avec cet email.");
            }
        }
    }
}
