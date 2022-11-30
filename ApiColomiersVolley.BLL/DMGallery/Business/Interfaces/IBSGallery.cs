using ApiColomiersVolley.BLL.DMGallery.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMGallery.Business.Interfaces
{
    public interface IBSGallery
    {
        CKFinderCreateFolderResult CreateFolder(CKFinderFolder folder, string newFolderName);
        string CreateImageDirectory();
        CKFinderDeleteResult DeleteFiles(IFormCollection form, CKFinderFolder folder);
        CKFinderDeleteResult DeleteFolder(CKFinderFolder folder);
        CKFinderFolder FindFolder(string currentFolder);
        CKFinderFilesResult GetCkfinderFiles(string type, string currentFolder);
        CKFinderFoldersResult GetCkfinderFolders(string currentFolder);
        CKFinderInitResult GetCkfinderInit();
        FileStreamResult GetFile(string type, string currentFolder, string fileName, string size, string command);
        CKFinderImageInfo GetImageInfo(string type, string currentFolder, string fileName);
        CkFinderMoveFileResult MoveFile(IFormCollection form, CKFinderFolder folder, bool copy);
        CKFinderRenameResult RenameFile(CKFinderFolder folder, string fileName, string newFileName);
        CKFinderSaveFileResult SaveFile(IFormCollection form, CKFinderFolder folder, string fileName);
        CKFinderSelectFileResult SelectFile(string folder, string fileName);
        Task<CKFinderUploadResult> UploadFile(IFormFileCollection files, CKFinderFolder folder);

    }
}
