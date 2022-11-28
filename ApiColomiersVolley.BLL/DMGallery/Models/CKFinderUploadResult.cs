using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMGallery.Models
{
    public class CKFinderUploadResult
    {
        public int uploaded { get; set; }
        public string url { get; set; }
        public CKFinderUploadError error { get; set; }

    }

    public class CKFinderUploadError
    {
        public string message { get; set; }
    }
}
