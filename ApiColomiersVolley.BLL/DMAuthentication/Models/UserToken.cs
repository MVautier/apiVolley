using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMAuthentication.Models
{
    public class UserToken
    {
        public string id_token { get; set; }
        public int IdUser { get; set; }
        public string refresh_token { get; set; }
        public DateTime expire_in { get; set; }
    }
}
