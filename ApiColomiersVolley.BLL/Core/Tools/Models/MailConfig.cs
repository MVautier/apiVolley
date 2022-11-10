using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.Core.Tools.Models
{
    public class MailConfig
    {
        public string srv_mail { get; set; }
        public int? port_mail { get; set; }
        public string user_mail { get; set; }
        public string pass_mail { get; set; }
        public string name_sender { get; set; }
        public string mail_sender { get; set; }
    }
}
