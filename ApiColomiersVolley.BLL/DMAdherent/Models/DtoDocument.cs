using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMAdherent.Models
{
    public class DtoDocument
    {
        public string filename { get; set; }
        public string type { get; set; }
        public Blob? blob { get; set; }
        public bool sent { get; set; }

    }
}
