using ApiColomiersVolley.BLL.Core.Tools.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.Core.Tools.Interfaces
{
    public interface IFileManager
    {
        PathConfig InitAdherentPaths(string id);
        FileInfo[] FindFiles(string path);
        DateTime GetDateLastModified(string path);
        FileInfo[] FindFiles(string path, string nameStart);
        FileInfo[] FindFiles(string path, string nameStart, string extension);
        Task<string[]> ReadFile(FileInfo file);
        Task<string> ReadFileAsString(FileInfo file);
        Task<byte[]> ReadFileAsBytes(FileInfo file);
        void MoveFile(string fullName, string destFullName);
        void DeleteFile(string fullName);
        Task CreateFile(string fullName, string content);
        Task CreateFile(string fullName, IFormFile file);
        void CreateFolder(string path);
        void CleanFolder(string path);
    }
}
