using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMGallery.Models
{
    public class CKFinderInitResult
    {
        public string c { get; set; }
        public bool enabled { get; set; }
        public CKFinderImageConfig images { get; set; }
        public List<CKFinderResourceType> resourceTypes { get; set; }
        public List<string> thumbs { get; set; }
        public bool uploadCheckImages { get; set; }
        public long uploadMaxSize { get; set; }
    }
}
