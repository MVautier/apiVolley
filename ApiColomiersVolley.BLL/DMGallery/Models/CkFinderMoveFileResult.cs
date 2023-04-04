
namespace ApiColomiersVolley.BLL.DMGallery.Models
{
    public class CkFinderMoveFileResult
    {
        public CKFinderFolder currentFolder { get; set; }
        public string resourceType { get; set; }
        public int moved { get; set; }
        public int copied { get; set; }
    }
}
