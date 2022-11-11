using ApiColomiersVolley.BLL.Core.Tools.Interfaces;
using ApiColomiersVolley.BLL.DMAuthentication.Business.Interfaces;
using ApiColomiersVolley.BLL.DMAuthentication.Exceptions;
using ApiColomiersVolley.BLL.DMAuthentication.Models;
using ApiColomiersVolley.BLL.DMAuthentication.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
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
        private readonly IDMConnexionRepo _connexionRepo;
        private readonly IDMTokenRepo _tokenRepo;
        private readonly IJWTFactory _jwtFactory;
        private readonly IEncryption _encryption;

        public BSAuthentication(IConfiguration config, IDMUserRepo userRepo, IDMConnexionRepo connexionRepo, IDMTokenRepo tokenRepo, IJWTFactory jwtFactory, IEncryption encryption)
        {
            _config = config;
            _userRepo = userRepo;
            _connexionRepo = connexionRepo;
            _tokenRepo = tokenRepo;
            _jwtFactory = jwtFactory;
            _encryption = encryption;
        }

        public async Task<UserToken> LogInUser(Login login, string ip)
        {
            var secureLogin = new Login
            {
                Email = login.Email,
                Password = _encryption.GeneratePasswordHash(login.Password)
            };

            var userInfo = await _userRepo.GetConnectingUser(secureLogin);
            if (userInfo == null)
            {
                await LogInvalidConnexion(login.Email, ip);
                return null;
            }

            await LogSuccessfulConnexion(userInfo.IdUser, ip);
            return await GenerateToken(userInfo, ip);
        }

        public async Task LogOutUser(string refreshId)
        {
            var hashRefresh = _encryption.GeneratePasswordHash(refreshId);
            var cnx = await GetConnexionByToken(hashRefresh);
            if (cnx == null)
            {
                await _tokenRepo.Delete(hashRefresh);
            }
            else
            {
                await _connexionRepo.Delete(cnx.IdConnexion);
                await _tokenRepo.DeleteByConnexion(cnx.IdConnexion);
            }
        }

        public async Task<UserToken> RefreshUser(Refresh refresh, string ip)
        {
            if (refresh.RefreshToken == null)
            {
                return null;
            }

            var hashRefresh = _encryption.GeneratePasswordHash(refresh.RefreshToken);
            if (!await _tokenRepo.Exists(hashRefresh))
            {
                return null;
            }

            var cnx = await GetConnexionByToken(hashRefresh);
            if (cnx == null || !cnx.IdUser.HasValue)
            {
                return null;
            }

            var userInfo = await _userRepo.GetRefreshUser(cnx.IdUser.Value);
            if (userInfo == null)
            {
                return null;
            }

            await LogSuccessfulConnexion(userInfo.IdUser, ip);
            return await GenerateToken(userInfo, ip);
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

        private async Task<UserToken> GenerateToken(UserInfo user, string ip)
        {
            var tokenConfig = _config.GetSection("Token");
            user.ExpireDate = DateTime.Now.AddMinutes(tokenConfig.GetValue<int>("ExpireAccess"));
            var jwtToken = _jwtFactory.GenerateJWT(user);
            return new UserToken
            {
                id_token = jwtToken,
                IdUser = user.IdUser,
                expire_in = DateTime.Now.AddMinutes(tokenConfig.GetValue<int>("ExpireRefresh")),
                refresh_token = await GenerateRefreshToken(user.IdUser, ip)
            };
        }

        private async Task<string> GenerateRefreshToken(int idUser, string ip)
        {

            var refreshId = Guid.NewGuid().ToString();
            var expireConfig = _config.GetSection("Token").GetValue<int>("ExpireRefresh");
            var token = new DtoToken
            {
                Key = _encryption.GeneratePasswordHash(refreshId),
                BeginDate = DateTime.Now,
                EndDate = DateTime.Now.AddMinutes(expireConfig)
            };

            token = await AddTokenConnexion(token, idUser, ip);
            if (token == null)
            {
                return "";
            }

            return refreshId;
        }

        private async Task<DtoToken> AddTokenConnexion(DtoToken token, int id, string ip)
        {
            var user = await _userRepo.GetById(id);
            if (user == null)
            {
                throw new ArgumentException("this user does not exist");
            }

            var cnx = await _connexionRepo.GetLastUserConnexion(id, ip);
            if (cnx == null)
            {
                cnx = await _connexionRepo.Add(new DtoConnexion
                {
                    BeginDate = DateTime.Now,
                    IdUser = id,
                    Ip = ip
                });
            }

            var newToken = token;
            newToken.IdConnexion = cnx.IdConnexion;
            return await _tokenRepo.Add(newToken);
        }

        private async Task LogInvalidConnexion(string login, string ip)
        {
            await _connexionRepo.Add(new DtoConnexion
            {
                BeginDate = DateTime.Now,
                Login = login,
                IdUser = null,
                Ip = ip,
                EndDate = null,
                LastRefresh = DateTime.Now
            });
        }

        private async Task<DtoConnexion> LogSuccessfulConnexion(int idUser, string ip)
        {
            var lastCnx = await _connexionRepo.GetLastUserConnexion(idUser, ip);
            var expireConfig = _config.GetSection("Token").GetValue<int>("ExpireRefresh");
            if (lastCnx != null && (lastCnx.EndDate == null || lastCnx.EndDate > DateTime.Now))
            {
                await _connexionRepo.IncrementRefresh(lastCnx.IdConnexion, DateTime.Now.AddMinutes(expireConfig));
            }

            if (lastCnx == null || lastCnx.EndDate < DateTime.Now)
            {
                lastCnx = await _connexionRepo.Add(new DtoConnexion
                {
                    BeginDate = DateTime.Now,
                    IdUser = idUser,
                    Ip = ip,
                    EndDate = DateTime.Now.AddMinutes(expireConfig),
                    LastRefresh = DateTime.Now
                });
            }

            return lastCnx;
        }

        private async Task<DtoConnexion> GetConnexionByToken(string tokenKey)
        {
            var token = await _tokenRepo.Get(tokenKey);
            if (token == null)
            {
                return null;
            }

            return await _connexionRepo.Get(token.IdConnexion);
        }
    }
}
