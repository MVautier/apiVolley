using System.Collections.Generic;

namespace ApiColomiersVolley.BLL.DMAdherent.Models
{
    public class DtoMailingRequest
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<string> Emails { get; set; }
    }
}
