using ApiColomiersVolley.BLL.DMGallery.Business.Interfaces;
using ApiColomiersVolley.BLL.DMGallery.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiColomiersVolley.Controllers
{
    /// <summary>
    /// Traitements de la galerie
    /// </summary>
    /// <response code="200">Success / Succès de la requête</response>
    /// <response code="204">No content / Aucune donnée</response>
    /// <response code="400">Bad request / La syntaxe de la requête est erronée</response>
    /// <response code="403">Forbidden / Accès refusé:  les droits d'accès ne permettent pas au client d'accéder à la ressource</response>
    /// <response code="500">Internal Server Error / Erreur interne du serveur</response>
    [ApiController]
    [Route("api/[controller]")]
    public class GalleryController : ControllerBase
    {
        private readonly IBSGallery _bsGallery;

        /// <summary>
        /// The class Constructor
        public GalleryController(IBSGallery bsGallery)
        {
            _bsGallery = bsGallery;
        }

        /// <summary>
        /// Executes a POST demand from CKFinder
        /// </summary>
        /// <param name="command">The command to execute</param>
        /// <param name="currentFolder">The current folder</param>
        /// <param name="type">The file type</param>
        /// <param name="fileName">The file name</param>
        /// <param name="newFileName">The new file name</param>
        /// <param name="newFolderName">The new folder name</param>
        /// <response code="200">Success / Succès de la requête</response>
        /// <response code="204">No content / Aucune donnée</response>
        /// <response code="400">Bad request / La syntaxe de la requête est erronée</response>
        /// <response code="403">Forbidden / Accès refusé:  les droits d'accès ne permettent pas au client d'accéder à la ressource</response>
        /// <response code="500">Internal Server Error / Erreur interne du serveur</response>
        [HttpPost]
        public async Task<IActionResult?> ImageUpload([FromQuery] string command, [FromQuery] string? currentFolder, [FromQuery] string? type, [FromQuery] string? fileName, [FromQuery] string? newFileName, [FromQuery] string? newFolderName)
        {
            // TODO : check user and is admin
            if (command == null)
            {
                return null;
            }

            CKFinderFolder folder = _bsGallery.FindFolder(currentFolder);
            IActionResult result = null;

            switch (command)
            {
                case "FileUpload":
                    result = Ok(await _bsGallery.UploadFile(Request.Form.Files, folder));
                    break;
                case "RenameFile":
                    result = Ok(_bsGallery.RenameFile(folder, fileName, newFileName));
                    break;
                case "MoveFiles":
                    result = Ok(_bsGallery.MoveFile(Request.Form, folder, false));
                    break;
                case "CopyFiles":
                    result = Ok(_bsGallery.MoveFile(Request.Form, folder, true));
                    break;
                case "DeleteFiles":
                    result = Ok(_bsGallery.DeleteFiles(Request.Form, folder));
                    break;
                case "CreateFolder":
                    result = Ok(_bsGallery.CreateFolder(folder, newFolderName));
                    break;
                case "DeleteFolder":
                    result = Ok(_bsGallery.DeleteFolder(folder));
                    break;
                case "SaveImage":
                    result = Ok(_bsGallery.SaveFile(Request.Form, folder, fileName));
                    break;
            }

            return result;
        }

        /// <summary>
        /// Executes a GET demand from CKFinder
        /// </summary>
        /// <param name="command">The command to execute</param>
        /// <param name="type">The file type</param>
        /// <param name="currentFolder">The current folder</param>
        /// <param name="fileName">The file name</param>
        /// <param name="size">The size in wwwxhhh format</param>
        /// <response code="200">Success / Succès de la requête</response>
        /// <response code="204">No content / Aucune donnée</response>
        /// <response code="400">Bad request / La syntaxe de la requête est erronée</response>
        /// <response code="403">Forbidden / Accès refusé:  les droits d'accès ne permettent pas au client d'accéder à la ressource</response>
        /// <response code="500">Internal Server Error / Erreur interne du serveur</response>
        [HttpGet]
        public async Task<IActionResult?> Get([FromQuery] string command, [FromQuery] string? type, [FromQuery] string? currentFolder, [FromQuery] string? fileName, [FromQuery] string? size)
        {
            // TODO : check user and is admin
            IActionResult result = null;
            if (command == null)
            {
                return null;
            }

            switch (command)
            {
                case "Init":
                    result = Ok(_bsGallery.GetCkfinderInit());
                    break;
                case "GetFolders":
                    result = Ok(_bsGallery.GetCkfinderFolders(currentFolder));
                    break;
                case "GetFiles":
                    result = Ok(_bsGallery.GetCkfinderFiles(type, currentFolder));
                    break;
                case "Thumbnail":
                case "ImagePreview":
                case "DownloadFile":
                    result = _bsGallery.GetFile(type, currentFolder, fileName, size, command);
                    break;
                case "ImageInfo":
                    result = Ok(_bsGallery.GetImageInfo(type, currentFolder, fileName));
                    break;
                case "SelectImage":
                    result = Ok(_bsGallery.SelectFile(currentFolder, fileName));
                    break;
            }
            return result;
        }
    }
}
