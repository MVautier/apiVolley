using ApiColomiersVolley.BLL.Core.Tools.Extensions;
using ApiColomiersVolley.BLL.Core.Tools.Interfaces;
using ApiColomiersVolley.BLL.Core.Tools.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.Core.Tools
{
    public class FileManager : IFileManager
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _config;

        public FileManager(IHostingEnvironment hostingEnvironment, IConfiguration config)
        {
            _hostingEnvironment = hostingEnvironment;
            _config = config;
        }

        public PathConfig InitAdherentPaths(string id)
        {
            var basePath = _hostingEnvironment.WebRootPath;
            var paths = _config.GetSection("Paths");
            var baseUrl = paths.GetValue<string>("BaseUrl");
            //string dirname = GetDirName(id);
            var adhBasePath = Path.Combine(basePath, paths.GetValue<string>("Adherent"), id);
            var adhBaseUrl = Path.Combine(baseUrl, paths.GetValue<string>("Adherent"), id);
            if (!Directory.Exists(adhBasePath))
            {
                Directory.CreateDirectory(adhBasePath);
            }

            return new PathConfig
            {
                BasePath = adhBasePath,
                BaseUrl = adhBaseUrl
            };
        }

        //private string GetDirName(string id)
        //{
        //    return (nom + "_" + prenom + "_" + tel).ToUpper().RemoveDiacritics();
        //}

        public FileInfo[] FindFiles(string path)
        {
            var directory = new DirectoryInfo(path);
            return directory.GetFiles();
        }

        public DateTime GetDateLastModified(string path)
        {
            var directory = new DirectoryInfo(path);
            return directory.GetFiles().Max(f => f.LastWriteTime);
        }

        public FileInfo[] FindFiles(string path, string nameStart)
        {
            var directory = new DirectoryInfo(path);
            return directory.GetFiles(nameStart + "*.*");
        }

        public FileInfo[] FindFiles(string path, string nameStart, string extension)
        {
            var directory = new DirectoryInfo(path);
            return directory.GetFiles(nameStart + "*." + extension);
        }

        public async Task<string[]> ReadFile(FileInfo file)
        {
            var lines = new List<string>();
            using (var stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    lines.Add(line);
                }
            }

            return lines.ToArray();
        }

        public async Task<string> ReadFileAsString(FileInfo file)
        {
            using (var reader = File.OpenText(file.FullName))
            {
                return await reader.ReadToEndAsync();
            }
        }

        public async Task<byte[]> ReadFileAsBytes(FileInfo file)
        {
            using (var reader = File.Open(file.FullName, FileMode.Open))
            {
                var result = new byte[reader.Length];
                await reader.ReadAsync(result, 0, (int)reader.Length);
                return result;
            }

        }

        public void MoveFile(string fullName, string destFullName)
        {
            var path = destFullName.Substring(0, destFullName.LastIndexOf('/'));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            DeleteFile(destFullName);
            File.Move(fullName, destFullName);
        }

        public void DeleteFile(string fullName)
        {
            if (File.Exists(fullName))
            {
                File.Delete(fullName);
            }
        }

        public async Task CreateFile(string fullName, string content)
        {
            DeleteFile(fullName);
            var buffer = Encoding.UTF8.GetBytes(content);
            using (var fileStream = new FileStream(fullName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, buffer.Length, true))
            {
                await fileStream.WriteAsync(buffer, 0, buffer.Length);
            }
        }

        public async Task CreateFile(string fullName, IFormFile file)
        {
            DeleteFile(fullName);
            using (var stream = new FileStream(fullName, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }

        public void CreateFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public void CleanFolder(string path)
        {
            if (Directory.Exists(path))
            {
                var files = Directory.EnumerateFiles(path).ToList();
                files.ForEach(f => DeleteFile(f));
            }
        }
    }
}
