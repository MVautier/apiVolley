using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMAuthentication.Models
{
    public  class UserInfo
    {
        public int? IdUser { get; set; }
        public int? IdUserUnlogged { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mail { get; set; }
        public DateTime ExpireDate { get; set; }
        public bool HasValidatedRegistration { get; set; }
        public Guid? GuidUser { get; set; }
        public string AccountName { get; set; }
        public bool HasActiveAccount { get; set; }
    }
}
