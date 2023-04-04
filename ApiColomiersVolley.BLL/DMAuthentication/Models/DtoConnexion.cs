using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMAuthentication.Models
{
    public class DtoConnexion
    {
        public int IdConnexion { get; set; }
        public string Login { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime LastRefresh { get; set; }
        public DateTime? EndDate { get; set; }
        public string Ip { get; set; }
        public int RefreshCount { get; set; }
        public int? IdUser { get; set; }
    }
}
