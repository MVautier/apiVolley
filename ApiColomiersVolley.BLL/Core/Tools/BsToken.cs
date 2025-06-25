using ApiColomiersVolley.BLL.Core.Tools.Interfaces;
using ApiColomiersVolley.BLL.Core.Tools.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.Core.Tools
{
    public class BsToken : IBsToken
    {

        private readonly IMemoryCache _memoryCache;
        /// <summary>
        /// key used for tokens in memory cache
        /// </summary>
        public readonly string cacheKey = "tokens";

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="memoryCache"></param>
        public BsToken(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Retrieves token stored for a source in memory
        /// </summary>
        /// <param name="source"></param>
        /// <returns>The token according the source</returns>
        /// <exception cref="NotImplementedException"></exception>
        public InMemoryToken GetToken(string source)
        {
            return _memoryCache.Get<InMemoryToken>(cacheKey + "_" + source);
        }

        /// <summary>
        /// Retrieves token stored in memory
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token"></param>
        /// <param name="refresh"></param>
        /// <param name="expiresAt"></param>
        /// <returns>The token stored in memory</returns>
        public InMemoryToken StoreToken(string source, string token, string refresh, DateTime expiresAt)
        {
            string key = cacheKey + "_" + source;
            InMemoryToken tok = new InMemoryToken
            {
                Source = source,
                Token = token,
                RefreshToken = refresh,
                ExpiresAt = expiresAt
            };
            MemoryCacheEntryOptions opt = new MemoryCacheEntryOptions
            {
                Priority = CacheItemPriority.NeverRemove
            };
            _memoryCache.Remove(key);
            _memoryCache.Set(key, tok, opt);
            return tok;
        }

        /// <summary>
        /// Removes token stored in memory
        /// </summary>
        /// <param name="source"></param>
        /// <returns>The token stored in memory</returns>
        public void RemoveToken(string source)
        {
            string key = cacheKey + "_" + source;
            _memoryCache.Remove(key);
        }
    }
}
