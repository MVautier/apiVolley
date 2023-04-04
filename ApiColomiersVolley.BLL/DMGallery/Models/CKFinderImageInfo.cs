using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMGallery.Models
{
    public class CKFinderImageInfo
    {
        public CKFinderFolder currentFolder { get; set; }
        public int height { get; set; }
        public int width { get; set; }
        public long size { get; set; }
        public string resourceType { get; set; }
    }
}
