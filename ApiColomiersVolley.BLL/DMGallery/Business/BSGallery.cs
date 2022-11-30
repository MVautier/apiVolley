using ApiColomiersVolley.BLL.DMGallery.Business.Interfaces;
using ApiColomiersVolley.BLL.DMGallery.Models;
using ApiColomiersVolley.BLL.DMGallery.Models.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
//using System.Drawing;
//using System.Drawing.Drawing2D;
//using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMGallery.Business
{
    public class BSGallery : IBSGallery
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        public BSGallery(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public CKFinderCreateFolderResult CreateFolder(CKFinderFolder folder, string newFolderName)
        {
            CKFinderCreateFolderResult result = null;

            if (folder != null && !string.IsNullOrEmpty(newFolderName))
            {
                string newFolder = Path.Combine(Environment.CurrentDirectory, folder.path + newFolderName);
                if (!Directory.Exists(newFolder))
                {
                    Directory.CreateDirectory(newFolder);
                    result = result = new CKFinderCreateFolderResult
                    {
                        currentFolder = folder,
                        created = 1,
                        resourceType = "Images",
                        newFolder = newFolderName
                    };
                }
            }

            return result;
        }

        public string CreateImageDirectory()
        {
            string path = _hostingEnvironment.WebRootPath;
            path = Path.Combine(path, "medias\\images\\");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        public CKFinderDeleteResult DeleteFiles(IFormCollection form, CKFinderFolder folder)
        {
            CKFinderDeleteResult result = null;
            CKFinderRequest deleteRequest = Newtonsoft.Json.JsonConvert.DeserializeObject<CKFinderRequest>(form["jsonData"]);
            if (folder != null && deleteRequest != null && deleteRequest.files.Count > 0)
            {
                int nbDeleted = 0;
                foreach (CKFinderFile file in deleteRequest.files)
                {
                    string path = Path.Combine(Environment.CurrentDirectory, folder.path + file.name);
                    string thumbnailpath = Path.Combine(Environment.CurrentDirectory, folder.path + "thumbnail\\" + file.name);
                    if (File.Exists(path))
                    {
                        try
                        {
                            File.Delete(path);
                            File.Delete(thumbnailpath);
                            nbDeleted++;
                        }
                        catch (Exception)
                        {
                        }
                    }
                }

                result = new CKFinderDeleteResult
                {
                    currentFolder = folder,
                    deleted = nbDeleted,
                    resourceType = "Images"
                };
            }

            return result;
        }

        public CKFinderDeleteResult DeleteFolder(CKFinderFolder folder)
        {
            CKFinderDeleteResult result = null;
            if (folder != null && Directory.Exists(folder.path))
            {
                Directory.Delete(folder.path, true);
                result = new CKFinderDeleteResult
                {
                    currentFolder = folder,
                    deleted = 1,
                    resourceType = "Images"
                };
            }

            return result;
        }

        public CKFinderFolder FindFolder(string currentFolder)
        {
            CKFinderFoldersResult folders = GetCkfinderFolders(currentFolder);
            CKFinderFolder folder = currentFolder != "/" && folders.folders.Any(f => f.path == currentFolder) ? folders.folders.FirstOrDefault(f => f.path == currentFolder) : folders.currentFolder;
            return folder;
        }

        public CKFinderFilesResult GetCkfinderFiles(string type, string currentFolder)
        {
            string serverpath = _configuration.GetSection("Paths").GetValue<string>("PathFileServ");
            CKFinderFolder folder = FindFolder(currentFolder);
            List<CKFinderFile> files = new List<CKFinderFile>();
            if (folder != null)
            {
                string path = Path.Combine(Environment.CurrentDirectory, folder.path);
                var items = Directory.GetFiles(path).Where(fi => !fi.EndsWith(".db"));
                if (items.Any())
                {
                    foreach (var item in items)
                    {
                        FileInfo info = new FileInfo(item);
                        files.Add(new CKFinderFile
                        {
                            folder = currentFolder,
                            date = info.LastWriteTime.ToString("yyyyMMddHHmmss"),
                            name = info.Name,
                            size = info.Length / 1024
                        });
                    }
                }
            }
            return new CKFinderFilesResult
            {
                resourceType = "Images",
                currentFolder = new CKFinderFolder
                {
                    acl = 1023,
                    path = currentFolder,
                    name = currentFolder,
                    url = serverpath + "medias/"
                },
                files = files
            };
        }

        public CKFinderFoldersResult GetCkfinderFolders(string currentFolder)
        {
            string serverpath = _configuration.GetSection("Paths").GetValue<string>("PathFileServ");
            string path = "wwwroot/medias" + currentFolder;
            string dossier = Path.Combine(Environment.CurrentDirectory, path);
            string baseUrl = serverpath + "medias";
            List<CKFinderFolder> folders = GetSubFolders(dossier, currentFolder, baseUrl);
            return new CKFinderFoldersResult
            {
                resourceType = "Images",
                currentFolder = new CKFinderFolder
                {
                    acl = 1023,
                    path = path,
                    hasChildren = folders.Count > 0,
                    name = currentFolder,
                    url = baseUrl + currentFolder
                },
                folders = folders
            };
        }

        public CKFinderInitResult GetCkfinderInit()
        {
            string serverpath = _configuration.GetSection("Paths").GetValue<string>("PathFileServ");
            return new CKFinderInitResult
            {
                c = "FD6FHE82DWH",
                enabled = true,
                images = new CKFinderImageConfig
                {
                    max = "1600x1200",
                    sizes = new CKFinderImageSize
                    {
                        large = "800x600",
                        medium = "600x480",
                        small = "480x320"
                    }
                },
                resourceTypes = new List<CKFinderResourceType>{
                    new CKFinderResourceType
                    {
                        allowedExtensions = "jpg,jpeg,png,gif,bmp,ico",
                        deniedExtensions = "",
                        hasChildren = true,
                        hash = "e4bb8edd9df46454",
                        maxSize = 2097152,
                        name = "Images",
                        url = serverpath + "medias"
                    }
                },
                thumbs = new List<string> { "150x150", "300x300", "500x500" },
                uploadCheckImages = false,
                uploadMaxSize = 2097152
            };
        }

        public FileStreamResult GetFile(string type, string currentFolder, string fileName, string size, string command)
        {
            CKFinderFolder folder = FindFolder(currentFolder);
            if (folder != null)
            {
                string path = Path.Combine(Environment.CurrentDirectory, folder.path + "/" + fileName);
                if (System.IO.File.Exists(path))
                {
                    FileInfo info = new FileInfo(path);
                    if (command == "Thumbnail" && !string.IsNullOrEmpty(size))
                    {
                        path = CreateThumbnail(info, size);
                    }

                    var data = System.IO.File.ReadAllBytes(path);
                    return new FileStreamResult(new MemoryStream(data), (command == "DownloadFile" ? "application/octet-stream" : "image/" + info.Extension.Replace(".", "")))
                    {
                        FileDownloadName = fileName
                    };
                }
            }
            return null;
        }

        public CKFinderImageInfo GetImageInfo(string type, string currentFolder, string fileName)
        {
            CKFinderFolder folder = FindFolder(currentFolder);
            if (folder != null)
            {
                string path = Path.Combine(Environment.CurrentDirectory, folder.path + fileName);
                if (File.Exists(path))
                {
                    FileInfo fileinfo = new FileInfo(path);
                    IImageInfo info = Image.Identify(path);
                    return new CKFinderImageInfo
                    {
                        currentFolder = folder,
                        height = info.Height,
                        width = info.Width,
                        size = fileinfo.Length / 1024,
                        resourceType = "Images"
                    };
                }
            }
            return null;
        }

        public CkFinderMoveFileResult MoveFile(IFormCollection form, CKFinderFolder folder, bool copy)
        {
            CkFinderMoveFileResult result = null;
            CKFinderRequest moveRequest = Newtonsoft.Json.JsonConvert.DeserializeObject<CKFinderRequest>(form["jsonData"]);
            if (folder != null && moveRequest != null && moveRequest.files.Count > 0)
            {
                int nbMoved = 0;
                int nbCopied = 0;
                foreach (CKFinderFile file in moveRequest.files)
                {
                    CKFinderFolder fileFolder = FindFolder(file.folder);
                    string path = Path.Combine(Environment.CurrentDirectory, fileFolder.path + file.name);
                    string destPath = Path.Combine(Environment.CurrentDirectory, folder.path + file.name);
                    if (File.Exists(path))
                    {
                        File.Copy(path, destPath, true);
                        if (copy)
                        {
                            nbCopied++;
                        }
                        else
                        {
                            try
                            {
                                File.Delete(path);
                                nbMoved++;

                            }
                            catch (Exception)
                            {
                            }
                        }
                    }

                }

                result = new CkFinderMoveFileResult
                {
                    currentFolder = folder,
                    moved = nbMoved,
                    copied = nbCopied,
                    resourceType = "Images"
                };
            }

            return result;
        }

        public CKFinderRenameResult RenameFile(CKFinderFolder folder, string fileName, string newFileName)
        {
            CKFinderRenameResult result = null;
            if (folder != null && !string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(newFileName))
            {
                string path = Path.Combine(Environment.CurrentDirectory, folder.path + fileName);
                string destPath = Path.Combine(Environment.CurrentDirectory, folder.path + newFileName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Copy(path, destPath);
                    try
                    {
                        System.IO.File.Delete(path);
                        result = new CKFinderRenameResult
                        {
                            currentFolder = folder,
                            name = fileName,
                            newName = newFileName,
                            renamed = 1,
                            resourceType = "Images"
                        };
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return result;
        }

        public CKFinderSaveFileResult SaveFile(IFormCollection form, CKFinderFolder folder, string fileName)
        {
            CKFinderSaveFileResult result = null;
            string base64 = form["content"].ToString();
            if (!string.IsNullOrEmpty(base64) && !string.IsNullOrEmpty(fileName))
            {
                string path = Path.Combine(Environment.CurrentDirectory, folder.path + fileName);
                Image img = Base64StringToImage(base64.Replace("data:image/png;base64,", ""));
                img.Save(path);
                img.Dispose();
                FileInfo info = new FileInfo(path);
                result = new CKFinderSaveFileResult
                {
                    currentFolder = folder,
                    saved = 1,
                    resourceType = "Images",
                    size = info.Length / 1024,
                    date = info.CreationTime.ToString("yyyyMMddHHmmss")
                };
            }

            return result;
        }

        public CKFinderSelectFileResult SelectFile(string currentFolder, string fileName)
        {
            CKFinderSelectFileResult result = null;
            CKFinderFolder folder = FindFolder(currentFolder);
            if (folder != null)
            {
                string path = Path.Combine(Environment.CurrentDirectory, folder.path + "\\" + fileName);
                if (File.Exists(path))
                {
                    string serverpath = _configuration.GetSection("Paths").GetValue<string>("PathFileServ");
                    FileInfo info = new FileInfo(path);
                    result = new CKFinderSelectFileResult
                    {
                        src = serverpath + folder.path.Replace("wwwroot/", "") + fileName,
                        saved = true,
                        size = info.Length / 1024,
                        date = info.CreationTime.ToString("yyyyMMddHHmmss")
                    };
                }
            }

            return result;
        }

        public async Task<CKFinderUploadResult> UploadFile(IFormFileCollection files, CKFinderFolder folder)
        {
            CKFinderUploadResult result = null;
            if (folder != null && files.Count > 0)
            {
                IFormFile file = files[0];
                string newFileName = file.FileName.Replace(" ", "");
                string path = Path.Combine(Environment.CurrentDirectory, folder.path + newFileName);
                using (Stream fileStream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                bool valid = await ImageIsValid(path);
                if (valid)
                {
                    result = new CKFinderUploadResult
                    {
                        uploaded = 1,
                        url = folder.url + file.Name
                    };
                }
                else
                {
                    File.Delete(path);
                    result = new CKFinderUploadResult
                    {
                        uploaded = 0,
                        url = string.Empty,
                        error = new CKFinderUploadError
                        {
                            message = "L'image n'est pas valide"
                        }
                    };
                }
            }
            else
            {
                result = new CKFinderUploadResult
                {
                    uploaded = 0,
                    url = string.Empty,
                    error = new CKFinderUploadError
                    {
                        message = "L'upload a échoué"
                    }
                };
            }

            return result;
        }

        private Image Base64StringToImage(string base64String)
        {
            Image result = null;
            //Convert Base64 string to byte[]
            byte[] byteBuffer = Convert.FromBase64String(base64String);
            using (MemoryStream memoryStream = new MemoryStream(byteBuffer))
            {
                memoryStream.Position = 0;
                result = Image.Load(memoryStream);
            }

            return result;
        }

        private string CreateThumbnail(FileInfo info, string size, bool force = false)
        {
            string thumbpath = _hostingEnvironment.WebRootPath + "\\medias\\thumbnail\\";
            if (!Directory.Exists(thumbpath))
            {
                Directory.CreateDirectory(thumbpath);
            }

            thumbpath += info.Name;
            if (!File.Exists(thumbpath) || force)
            {
                Image img = Image.Load(info.FullName);
                int w = img.Width;
                int h = img.Height;
                string[] sizes = size.Split(new char[] { 'x' }, StringSplitOptions.None);
                int resizew = Convert.ToInt32(sizes[0]);
                int resizeh = Convert.ToInt32(sizes[1]);
                if (w > resizew || h > resizeh)
                {
                    Image thumb = ResizeImage(img, resizew, resizeh);
                    thumb.Save(thumbpath);
                }
                else
                {
                    img.Save(thumbpath);
                }

                img.Dispose();
            }

            return thumbpath;
        }

        private List<CKFinderFolder> GetSubFolders(string path, string currentFolder, string baseUrl)
        {
            List<CKFinderFolder> folders = new List<CKFinderFolder>();
            List<string> subs = Directory.GetDirectories(path).ToList();
            if (subs.Any())
            {
                foreach (string d in subs)
                {
                    DirectoryInfo info = new DirectoryInfo(d);
                    if (info.Name != "thumbnail")
                    {
                        string[] dirs = Directory.GetDirectories(d);
                        bool hasChildren = dirs.Any() ? dirs.Count(dir => new DirectoryInfo(dir).Name != "thumbnail") > 0 : false;
                        folders.Add(new CKFinderFolder
                        {
                            acl = 1023,
                            path = path + info.Name,
                            hasChildren = hasChildren,
                            name = info.Name,
                            url = baseUrl + currentFolder + info.Name
                        });
                    }
                }
            }
            return folders;
        }

        public async Task<bool> ImageIsValid(string path)
        {
            using (Stream file = new FileStream(path, FileMode.Open))
            {
                return await ImageIsValid(file);
            }
        }

        public async Task<bool> ImageIsValid(Stream file)
        {
            bool valid = false;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    byte[] bytes = ms.ToArray();
                    string base64 = Convert.ToBase64String(bytes);
                    valid = bytes.IsImage() && base64.Length < 2147483647;
                }
            }
            catch (Exception)
            {
                valid = false;
            }
            finally
            {
                if (file != null) file.Close();
            }

            return valid;
        }

        private Image ResizeImage(Image image, int width, int height)
        {
            return image.Clone(x => x.Resize(width, height));
        }
    }
}
