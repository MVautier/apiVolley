using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMAuthentication.Models
{
    public class Login
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ClientID { get; set; }
    }
}
