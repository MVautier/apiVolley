using ApiColomiersVolley.BLL.Core.Tools.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMFile.Business.Interfaces
{
    public interface IBSDocument
    {
        Task<string> SaveDocument(string filename, string id, IFormFile file);
        Task<bool> SaveDocuments(string id, IFormFileCollection files);
        Task<FileModel> DownloadFile(string fileName, string uid);
    }
}
