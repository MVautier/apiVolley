using ApiColomiersVolley.BLL.Core.Tools.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.Core.Tools
{
    public class Encryption : IEncryption
    {
        private readonly IConfiguration _config;

        public Encryption(IConfiguration config)
        {
            _config = config;
        }

        public string GeneratePasswordHash(string value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            var hash = new HMACSHA256(Encoding.UTF8.GetBytes(_config.GetSection("Encryption").GetValue<string>("SaltKey")));
            var converted = hash.ComputeHash(Encoding.UTF8.GetBytes(value));
            return Convert.ToBase64String(converted);
        }

        public bool CompareWithPasswordHash(string value, string hash)
        {
            if (value == null || hash == null)
            {
                return false;
            }

            return GeneratePasswordHash(value) == hash;
        }

        public string DecryptElement(string hash)
        {
            return DecryptStringFromBytes(Convert.FromBase64String(hash));
        }

        private string DecryptStringFromBytes(byte[] cipherText)
        {
            if (cipherText == null || cipherText.Length <= 0)
            {
                return null;
            }

            var key = Encoding.UTF8.GetBytes(_config.GetSection("Encryption").GetValue<string>("DecryptKey"));
            var iv = Encoding.UTF8.GetBytes(_config.GetSection("Encryption").GetValue<string>("DecryptKey"));
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;
                rijAlg.Key = key;
                rijAlg.IV = iv;

                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
