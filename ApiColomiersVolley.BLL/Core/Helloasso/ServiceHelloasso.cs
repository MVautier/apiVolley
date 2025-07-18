﻿using ApiColomiersVolley.BLL.Core.Helloasso.Interfaces;
using ApiColomiersVolley.BLL.Core.Helloasso.Models;
using ApiColomiersVolley.BLL.Core.Tools.Interfaces;
using ApiColomiersVolley.BLL.Core.Tools.Models;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System.Net;

namespace ApiColomiersVolley.BLL.Core.Helloasso
{
    public class ServiceHelloasso : IServiceHelloasso
    {

        private readonly IConfiguration _config;
        private readonly IBsRequestApi _requestApi;
        private readonly IServiceSendMail _mailManager;
        private readonly IBsToken _bsToken;
        private const string FOURNISSEUR = "Helloasso";

        public ServiceHelloasso(IBsToken bsToken, IBsRequestApi requestApi, IServiceSendMail mailManager, IConfiguration config)
        {
            _bsToken = bsToken;
            _requestApi = requestApi;
            _mailManager = mailManager;
            _config = config;
        }

        public async Task<PostIntentResult> SendCheckout(Cart cart)
        {
            var config = _config.GetSection(FOURNISSEUR);
            string route = config["apiServer"] + "/organizations/" + config["organizationSlug"] + "/checkout-intents";
            var token = await GetToken();
            PaymentRequest data = new PaymentRequest(cart, config["itemName"], config["basePath"]);
            var result = await _requestApi.PostJsonWithToken<PostIntentResult>(FOURNISSEUR, route, token, data);
            return result;
            //PostIntentResult? result = null;
            //var config = _config.GetSection(FOURNISSEUR);
            //try
            //{

            //    var token = await GetToken();
            //    string route = config["apiServer"] + "/organizations/" + config["organizationSlug"] + "/checkout-intents";
            //    var client = GetClient(route);
            //    var request = new RestRequest("");
            //    request.AddHeader("accept", "application/json");
            //    request.AddHeader("authorization", "Bearer " + token);
            //    PaymentRequest data = new PaymentRequest(cart, config["itemName"], config["basePath"]);
            //    //request.AddBody(data);
            //    //string json = JsonConvert.SerializeObject(data);
            //    request.AddStringBody("{\"containsDonation\":\"false\",\"totalAmount\":9300,\"initialAmount\":9300,\"itemName\":\"Adhésion CLLL - Section Voley-Ball\",\"backUrl\":\"https://localhost:4224/inscription?step=4&payment=cancel\",\"errorUrl\":\"https://localhost:4224/inscription?step=4&payment=error\",\"returnUrl\":\"https://localhost:4224/inscription?step=4&payment=success\"}", "application/json");
            //    result = await client.PostAsync<PostIntentResult>(request);
            //    if (result != null)
            //    {
            //        return result;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new UnauthorizedAccessException("Unable to post intent", ex);
            //}

            //return null;
        }

        public async Task<GetIntentResult> GetReceiptUrl(string id)
        {
            //GetIntentResult? result = null;
            //var config = _config.GetSection(FOURNISSEUR);
            //string route = config["apiServer"] + "/organizations/" + config["organizationSlug"] + "/checkout-intents/" + id;
            //try
            //{
            //    var token = await GetToken();
            //    var client = GetClient(route);
            //    var request = new RestRequest("");
            //    request.AddHeader("accept", "application/json");
            //    request.AddHeader("authorization", "Bearer " + token);
            //    result = await client.GetAsync<GetIntentResult>(request);
            //    if (result != null)
            //    {
            //        return result;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new UnauthorizedAccessException("Unable to get intent", ex);
            //}

            //return null;

            var config = _config.GetSection(FOURNISSEUR);
            string route = config["apiServer"] + "/organizations/" + config["organizationSlug"] + "/checkout-intents/" + id;
            var token = await GetToken();
            var result = await _requestApi.CallWithToken<GetIntentResult>(FOURNISSEUR, route, token, HttpMethod.Get);
            return result;
        }

        #region token
        private RestClient GetClient(string route)
        {
            var options = new RestClientOptions(route);
            if (_config.GetValue<string>("Environment") == "production")
            {
                options.Proxy = new WebProxy("http://winproxy.server.lan:3128/", true);
            }

            var client = new RestClient(options);
            return client;
        }

        private async Task<string> GetToken()
        {
            var tokenInMemory = _bsToken.GetToken(FOURNISSEUR);
            if (tokenInMemory == null)
            {
                tokenInMemory = await GetTokenByApi();
            }

            if (tokenInMemory != null && tokenInMemory.ExpiresAt < DateTime.Now)
            {
                tokenInMemory = await RefreshToken(tokenInMemory.RefreshToken);
            }

            if (tokenInMemory == null || string.IsNullOrEmpty(tokenInMemory.Token))
            {
                _bsToken.RemoveToken(FOURNISSEUR);
                tokenInMemory = await GetTokenByApi();
            }

            if (tokenInMemory == null || string.IsNullOrEmpty(tokenInMemory.Token))
            {
                throw new UnauthorizedAccessException($"{FOURNISSEUR} token is null");
            }

            return tokenInMemory.Token;
        }

        public async Task<InMemoryToken> GetTokenByApi()
        {
            InMemoryToken tokenInMemory = null;
            var config = _config.GetSection(FOURNISSEUR);
            try
            {
                string route = config["authServer"] + "/token";
                var client = GetClient(route);
                var request = new RestRequest("");
                request.AddHeader("accept", "application/json");
                request.AddParameter("grant_type", "client_credentials");
                request.AddParameter("client_id", config["clientId"]);
                request.AddParameter("client_secret", config["clientSecret"]);
                var response = await client.PostAsync<HelloassoToken>(request);
                if (response != null)
                {
                    DateTime expires = GetExpires(config);
                    tokenInMemory = _bsToken.StoreToken(FOURNISSEUR, response.access_token, response.refresh_token, expires);
                    return tokenInMemory;
                }
            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException("Unable to obtain token", ex);
            }

            return null;
        }

        private async Task<InMemoryToken> RefreshToken(string refresh)
        {
            var config = _config.GetSection(FOURNISSEUR);
            string route = config["authServer"] + "/token";
            var client = GetClient(route);
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            request.AddParameter("grant_type", "refresh_token");
            try
            {
                var response = await client.PostAsync<HelloassoToken>(request);
                if (response != null)
                {
                    DateTime expires = GetExpires(config);
                    return _bsToken.StoreToken(FOURNISSEUR, response.access_token, response.refresh_token, expires);
                }
            }
            catch (Exception ex)
            {
                _mailManager.SendMailErreur(ex, "Unable to refresh token");
            }

            return null;
            //var dict = new Dictionary<string, string>();
            //dict.Add("client_id", config["clientId"]);
            //dict.Add("refresh_token", refresh);
            //dict.Add("grant_type", "refresh_token");
            //var data = new FormUrlEncodedContent(dict);
            //try
            //{
            //    var result = await _requestApi.PostFormData<HelloassoToken>(FOURNISSEUR, config["authServer"] + "/token", data);
            //    if (result != null)
            //    {
            //        DateTime expires = GetExpires(config);
            //        return _bsToken.StoreToken(FOURNISSEUR, result.access_token, result.refresh_token, expires);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    _mailManager.SendMailErreur(ex, "Unable to refresh token");
            //}

            //return null;
        }

        private DateTime GetExpires(IConfigurationSection config)
        {
            DateTime expires = DateTime.Now;
            if (int.TryParse(config["expires_in"], out int duration))
            {
                expires = DateTime.Now.AddSeconds(duration);
            }

            return expires;
        }
        #endregion
    }
}
