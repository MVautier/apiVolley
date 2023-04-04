using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMGallery.Models
{
    public class CKFinderResourceType
    {
        public string allowedExtensions { get; set; }
        public string deniedExtensions { get; set; }
        public bool hasChildren { get; set; }
        public string hash { get; set; }
        public long maxSize { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }
}
