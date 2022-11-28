using ApiColomiersVolley.BLL.DMGallery.Business.Interfaces;
using ApiColomiersVolley.BLL.DMGallery.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMGallery.Business
{
    public class BSGallery : IBSGallery
    {
        public CKFinderCreateFolderResult CreateFolder(CKFinderFolder folder, string newFolderName)
        {
            throw new NotImplementedException();
        }

        public string CreateImageDirectory(string guidCompte, int idCampagne)
        {
            throw new NotImplementedException();
        }

        public CKFinderDeleteResult DeleteFiles(IFormCollection form, CKFinderFolder folder)
        {
            throw new NotImplementedException();
        }

        public CKFinderDeleteResult DeleteFolder(CKFinderFolder folder)
        {
            throw new NotImplementedException();
        }

        public CKFinderFolder FindFolder(string currentFolder, string guidCompte)
        {
            throw new NotImplementedException();
        }

        public CKFinderFilesResult GetCkfinderFiles(string type, string currentFolder, string guidCompte)
        {
            throw new NotImplementedException();
        }

        public CKFinderFoldersResult GetCkfinderFolders(string currentFolder, string guidCompte)
        {
            throw new NotImplementedException();
        }

        public CKFinderInitResult GetCkfinderInit(string guidCompte)
        {
            throw new NotImplementedException();
        }

        public FileStreamResult GetFile(string type, string currentFolder, string fileName, string size, string command, string guidCompte)
        {
            throw new NotImplementedException();
        }

        public CKFinderImageInfo GetImageInfo(string type, string currentFolder, string fileName, string guidCompte)
        {
            throw new NotImplementedException();
        }

        public CkFinderMoveFileResult MoveFile(IFormCollection form, CKFinderFolder folder, bool copy, string guidCompte)
        {
            throw new NotImplementedException();
        }

        public CKFinderRenameResult RenameFile(CKFinderFolder folder, string fileName, string newFileName)
        {
            throw new NotImplementedException();
        }

        public CKFinderSaveFileResult SaveFile(IFormCollection form, CKFinderFolder folder, string fileName)
        {
            throw new NotImplementedException();
        }

        public CKFinderSelectFileResult SelectFile(string folder, string fileName, string guidCompte, int idCampagne)
        {
            throw new NotImplementedException();
        }

        public Task<CKFinderUploadResult> UploadFile(IFormFileCollection files, CKFinderFolder folder)
        {
            throw new NotImplementedException();
        }
    }
}
