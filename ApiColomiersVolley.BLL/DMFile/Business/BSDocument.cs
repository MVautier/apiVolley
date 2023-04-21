using ApiColomiersVolley.BLL.Core.Tools.Interfaces;
using ApiColomiersVolley.BLL.DMAdherent.Repositories;
using ApiColomiersVolley.BLL.DMFile.Business.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMFile.Business
{
    public class BSDocument : IBSDocument
    {
        private readonly IFileManager _fileManager;

        public BSDocument(IFileManager fileManager)
        {
            _fileManager = fileManager;
        }

        public async Task<string> SavePdf(string filename, string id, IFormFile file)
        {
            var paths = _fileManager.InitAdherentPaths(id);
            await _fileManager.CreateFile(Path.Combine(paths.BasePath, filename), file);
            return Path.Combine(paths.BaseUrl, filename);
        }

        public async Task<bool> SaveDocuments(string id, IFormFileCollection files)
        {
            try
            {
                var paths = _fileManager.InitAdherentPaths(id);
                foreach (var file in files)
                {
                    await _fileManager.CreateFile(Path.Combine(paths.BasePath, file.FileName), file);
                }

                return true;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                return false;
            }
            
        }
    }
}
