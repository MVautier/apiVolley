using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMGallery.Models
{
    public class CKFinderFolder
    {
        public int acl { get; set; }
        public bool hasChildren { get; set; }
        public string path { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }
}
