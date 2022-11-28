using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMGallery.Models
{
    public class CKFinderDeleteResult
    {
        public CKFinderFolder currentFolder { get; set; }
        public string resourceType { get; set; }
        public int deleted { get; set; }
    }
}
