using ApiColomiersVolley.BLL.Core.Tools.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace ApiColomiersVolley.BLL.Core.Tools
{
    public class JWTFactory : IJWTFactory
    {
        private readonly IConfiguration _config;

        public JWTFactory(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateJWT<T>(T value)
        {
            var tokenConfig = _config.GetSection("Token");
            return GenerateJWT(value, tokenConfig.GetValue<string>("JwtKey"));
        }

        public T CheckAndExtract<T>(string token)
        {
            var tokenConfig = _config.GetSection("Token");
            var key = tokenConfig.GetValue<string>("JwtKey");
            if (!CheckJWT(token, key))
            {
                throw new Exceptions.CheckJwtException();
            }
            return GetValueFromJWT<T>(token, key);
        }

        private string GenerateJWT<T>(T obj, string saltKey)
        {
            var header = new JwtPayload
            {
                {"alg","HS256" },
                {"typ","JWT"}
            };
            var payload = new JwtPayload
            {
                {"value", obj }
            };
            string info = header.Base64UrlEncode() + "." + payload.Base64UrlEncode();
            byte[] secret = Encoding.UTF8.GetBytes(saltKey);
            var bytesToSign = Encoding.UTF8.GetBytes(info);
            var alg = new HMACSHA256(secret);
            var hash = alg.ComputeHash(bytesToSign);
            var computedSignature = Base64UrlEncode(hash);
            return info + "." + computedSignature;
        }

        private string Base64UrlEncode(byte[] input)
        {
            var output = Convert.ToBase64String(input);
            output = output.Split('=')[0]; // Remove any trailing '='s
            output = output.Replace('+', '-'); // 62nd char of encoding
            output = output.Replace('/', '_'); // 63rd char of encoding
            return output;
        }

        private bool CheckJWT(string token, string saltKey)
        {
            string[] parts = token.Split(".".ToCharArray());
            if (parts.Length == 3)
            {
                string header = parts[0];
                string payload = parts[1];
                string signature = parts[2];//Base64UrlEncoded signature from the token

                byte[] bytesToSign = Encoding.UTF8.GetBytes(string.Join(".", header, payload));
                byte[] secret = Encoding.UTF8.GetBytes(saltKey);
                HMACSHA256 alg = new HMACSHA256(secret);
                byte[] hash = alg.ComputeHash(bytesToSign);
                string computedSignature = Base64UrlEncode(hash);
                return signature == computedSignature;
            }
            else
            {
                throw new ArgumentException("Invalid token", "token");
            }
        }

        private T GetValueFromJWT<T>(string token, string saltKey)
        {
            if (!CheckJWT(token, saltKey))
            {
                throw new ArgumentException("Invalid signature Token", "token");
            }

            JwtSecurityToken jwtToken = new JwtSecurityToken(token);
            if (!jwtToken.Payload.ContainsKey("value"))
            {
                throw new ArgumentException("Missing value in token", "token");
            }

            return JsonConvert.DeserializeObject<T>(jwtToken.Payload["value"].ToString());
        }
    }
}
