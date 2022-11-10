using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.Core.Tools.Models
{
    public class MailAttachementTemplate
    {
        public string Name { get; set; }
        public string Filename { get; set; }
        public string ContentType { get; set; }
        public string TransferEncoding { get; set; }
        public string Type { get; set; }
    }
}
