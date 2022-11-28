using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMGallery.Models
{
    public class CKFinderRenameResult
    {
        public CKFinderFolder currentFolder { get; set; }
        public string name { get; set; }
        public string newName { get; set; }
        public int renamed { get; set; }
        public string resourceType { get; set; }
    }
}
