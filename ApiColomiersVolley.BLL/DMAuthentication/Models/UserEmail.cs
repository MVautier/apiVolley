using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMAuthentication.Models
{
    public class UserEmail
    {
        public string Email { get; set; }
        public string ClientID { get; set; }
        public bool ConsentNewsLetters { get; set; }
    }
}
