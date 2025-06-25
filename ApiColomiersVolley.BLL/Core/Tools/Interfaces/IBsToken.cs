using ApiColomiersVolley.BLL.Core.Tools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.Core.Tools.Interfaces
{
    public interface IBsToken
    {
        /// <summary>
        /// Retrieves token stored for a source in memory
        /// </summary>
        /// <param name="source"></param>
        /// <returns>The token according the source</returns>
        /// <exception cref="NotImplementedException"></exception>
        InMemoryToken GetToken(string source);

        /// <summary>
        /// Retrieves token stored in memory
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token"></param>
        /// <param name="refresh"></param>
        /// <param name="expiresAt"></param>
        /// <returns>The token stored in memory</returns>
        InMemoryToken StoreToken(string source, string token, string refresh, DateTime expiresAt);

        /// <summary>
        /// Removes token stored in memory
        /// </summary>
        /// <param name="source"></param>
        /// <returns>The token stored in memory</returns>
        void RemoveToken(string source);
    }
}
