using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.Core.Tools.Models
{
    public class InMemoryToken
    {
        /// <summary>
        /// Source of the token
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Authentication token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Token for refreshing authentication
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Token expiration time
        /// </summary>
        public DateTime ExpiresAt { get; set; }
    }
}
