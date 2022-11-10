using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.Core.Tools.Models
{
    public class MailTemplate
    {
        public string Code { get; set; }
        public string Text { get; set; }
        public string HTML { get; set; }
        public string Subject { get; set; }
        public List<MailAttachementTemplate> Files { get; set; }
    }
}
