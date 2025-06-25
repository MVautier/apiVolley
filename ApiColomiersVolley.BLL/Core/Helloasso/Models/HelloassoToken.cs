using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.Core.Helloasso.Models
{
    public class HelloassoToken
    {
        /// <summary>
        /// Token that must be provided in each call to api (valid for 15 min)
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// Token to be used for renew authentication
        /// </summary>
        public string refresh_token { get; set; }
    }
}
