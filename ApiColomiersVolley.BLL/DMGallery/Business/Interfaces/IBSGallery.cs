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
        CKFinderInitResult GetCkfinderInit(string guidCompte);
        CKFinderFoldersResult GetCkfinderFolders(string currentFolder, string guidCompte);
        CKFinderFilesResult GetCkfinderFiles(string type, string currentFolder, string guidCompte);
        FileStreamResult GetFile(string type, string currentFolder, string fileName, string size, string command, string guidCompte);
        CKFinderImageInfo GetImageInfo(string type, string currentFolder, string fileName, string guidCompte);
        Task<CKFinderUploadResult> UploadFile(IFormFileCollection files, CKFinderFolder folder);
        CKFinderFolder FindFolder(string currentFolder, string guidCompte);
        CKFinderRenameResult RenameFile(CKFinderFolder folder, string fileName, string newFileName);
        CkFinderMoveFileResult MoveFile(IFormCollection form, CKFinderFolder folder, bool copy, string guidCompte);
        CKFinderDeleteResult DeleteFiles(IFormCollection form, CKFinderFolder folder);
        CKFinderDeleteResult DeleteFolder(CKFinderFolder folder);
        CKFinderCreateFolderResult CreateFolder(CKFinderFolder folder, string newFolderName);
        CKFinderSaveFileResult SaveFile(IFormCollection form, CKFinderFolder folder, string fileName);
        CKFinderSelectFileResult SelectFile(string folder, string fileName, string guidCompte, int idCampagne);
        string CreateImageDirectory(string guidCompte, int idCampagne);
    }
}
