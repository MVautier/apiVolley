using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMGallery.Models
{
    public class CKFinderCreateFolderResult
    {
        public CKFinderFolder currentFolder { get; set; }
        public string newFolder { get; set; }
        public int created { get; set; }
        public string resourceType { get; set; }
    }
}
