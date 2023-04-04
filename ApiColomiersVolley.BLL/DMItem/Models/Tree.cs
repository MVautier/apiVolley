using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMItem.Models
{
    public class Tree
    {
        public List<WebItem> pages  { get; set; }
        public List<WebItem> posts { get; set; }
        public List<WebItem> comments { get; set; }
    }
}
