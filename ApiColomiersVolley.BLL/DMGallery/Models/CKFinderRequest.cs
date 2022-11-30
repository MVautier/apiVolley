using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMGallery.Models
{
    public class CKFinderRequest
    {
        public List<CKFinderFile> files { get; set; }
        public string ckCsrfToken { get; set; }
    }
}
