using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.Core.Tools.Models
{
    public class MailOrder
    {
        public string HtmlContent { get; set; }
        public string TextContent { get; set; }
        public string ToMail { get; set; }
        public List<string> ToMails { get; set; }
        public string Subject { get; set; }
        public List<MailAttachementTemplate> Files { get; set; }
    }
}
