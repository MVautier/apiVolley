using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMGallery.Models
{
    public class CKFinderFilesResult
    {
        public CKFinderFolder currentFolder { get; set; }
        public List<CKFinderFile> files { get; set; }
        public string resourceType { get; set; }
    }
}
