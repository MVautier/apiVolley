using ApiColomiersVolley.BLL.Core.Tools.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace ApiColomiersVolley.BLL.Core.Tools
{
    public class BsRequestApi : IBsRequestApi
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _config;

        /// <summary>
        /// Constructeur de la classe
        /// </summary>
        /// <param name="clientFactory"></param>
        /// <param name="config"></param>
        public BsRequestApi(IHttpClientFactory clientFactory, IConfiguration config)
        {
            _clientFactory = clientFactory;
            _config = config;
        }

        private HttpClient GetClient(string fournisseur)
        {
            HttpClient client;
            if (_config.GetValue<string>("Environment") == "production")
            {
                var proxiedHttpClientHandler = new HttpClientHandler() { UseProxy = true };
                proxiedHttpClientHandler.Proxy = new WebProxy("http://winproxy.server.lan:3128/", true);
                client = new HttpClient(proxiedHttpClientHandler);
            }
            else
            {
                client = _clientFactory.CreateClient();
            }

            var config = _config.GetSection(fournisseur);
            Uri baseUri = new Uri(config["apiServer"]);
            client.BaseAddress = baseUri;
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            
            return client;
        }

        /// <summary>
        /// Effectue un requête avec la méthode souhaitée en fournissant un Bearer token
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fournisseur"></param>
        /// <param name="route"></param>
        /// <param name="token"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public async Task<T> CallWithToken<T>(string fournisseur, string route, string token, HttpMethod method)
        {
            var client = GetClient(fournisseur);
            var requestMessage = new HttpRequestMessage(method, route);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsAsync<T>();
            return responseBody;
        }

        /// <summary>
        /// Effectue un requête POST au format FormData en fournissant un clientId et un clientSecret
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fournisseur"></param>
        /// <param name="route"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <returns></returns>
        public async Task<T> PostForm<T>(string fournisseur, string route, string clientId, string clientSecret)
        {
            var client = GetClient(fournisseur);
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("POST"), route);
            request.Content = new StringContent(string.Join("&", new[] { $"grant_type={"client_credentials"}", $"scope={"diamond"}" }));
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");
            string token = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", token);
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return (await response.Content.ReadAsAsync<T>());
        }

        /// <summary>
        /// Effectue un requête POST au format json en fournissant un Bearer token
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fournisseur"></param>
        /// <param name="route"></param>
        /// <param name="token"></param>
        /// <param name="body"></param>
        /// <param name="statusToCheck"></param>
        /// <returns></returns>
        public async Task<T> PostJsonWithToken<T>(string fournisseur, string route, string token, object body, HttpStatusCode statusToCheck = HttpStatusCode.NoContent)
        {
            var client = GetClient(fournisseur);
            
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, route);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.Default, "application/json");
            requestMessage.Content = content;
            var response = await client.SendAsync(requestMessage);
            if (response.StatusCode == statusToCheck)
            {
                return default(T);
            }

            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsAsync<T>();
            return responseBody;
        }

        /// <summary>
        /// Effectue un requête POST au format FormData en fournissant un Bearer token
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fournisseur"></param>
        /// <param name="route"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<T> PostFormData<T>(string fournisseur, string route, FormUrlEncodedContent data)
        {
            var client = GetClient(fournisseur);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, route);
            requestMessage.Content = data;
            var response = await client.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsAsync<T>();
            return responseBody;
        }

        /// <summary>
        /// Effectue un requête POST au format FormData en fournissant un Bearer token
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fournisseur"></param>
        /// <param name="route"></param>
        /// <param name="token"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<T> PostFormDataWithToken<T>(string fournisseur, string route, string? token, FormUrlEncodedContent data)
        {
            var client = GetClient(fournisseur);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, route);
            if (!string.IsNullOrEmpty(token))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            requestMessage.Content = data;
            var response = await client.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsAsync<T>();
            return responseBody;
        }
    }
}
