using ApiColomiersVolley.BLL.Core.Tools.Extensions;
using ApiColomiersVolley.BLL.Core.Tools.Interfaces;
using ApiColomiersVolley.BLL.Core.Tools.Models;
using ApiColomiersVolley.BLL.DMAdherent.Models;
using Cartegie.BLL.CreationDeFichiers.Interface;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.Core.Tools
{
    public class FileManager : IFileManager
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _config;
        private readonly IFileExport _fileExport;

        public FileManager(IHostingEnvironment hostingEnvironment, IConfiguration config, IFileExport fileExport)
        {
            _hostingEnvironment = hostingEnvironment;
            _config = config;
            _fileExport = fileExport;
        }

        public PathConfig InitAdherentPaths(string id, bool createDefault = true)
        {
            var basePath = _hostingEnvironment.WebRootPath;
            var paths = _config.GetSection("Paths");
            var baseUrl = paths.GetValue<string>("BaseUrl");
            //string dirname = GetDirName(id);
            var adhBasePath = Path.Combine(basePath, paths.GetValue<string>("Adherent"), id);
            var adhBaseUrl = Path.Combine(baseUrl, paths.GetValue<string>("Adherent"), id);
            if (!Directory.Exists(adhBasePath) && createDefault)
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
            if (!Directory.Exists(path))
            {
                return null;
            }

            var directory = new DirectoryInfo(path);
            return directory.GetFiles();
        }

        public string GetTypeByName(string name)
        {
            return 
                name.Contains("photo") ? "photo" : 
                name.Contains("certificat") ? "certificat" : 
                name.Contains("attestation") ? "attestation" : 
                name.Contains("adhesion") ? "adhesion" : 
                name.Contains("autorisation") ? "autorisation" :
                name.Contains("signature") ? "signature" : "";
        }

        public string CreateZipFile(string filename, List<DtoDocument> files)
        {
            string zipName = null;
            var basePath = _hostingEnvironment.WebRootPath;
            var paths = _config.GetSection("Paths");
            //string dirname = GetDirName(id);
            var exportBasePath = Path.Combine(basePath, paths.GetValue<string>("Export"));
            var adhBasePath = Path.Combine(basePath, paths.GetValue<string>("Adherent"));

            if (!string.IsNullOrEmpty(filename))
            {
                zipName = Path.Combine(exportBasePath, filename + ".zip");
                FileStream fsOut = File.Create(zipName);
                ZipOutputStream zipStream = new ZipOutputStream(fsOut);
                zipStream.SetLevel(9);// ranges 0 to 9 ... 0 = no compression : 9 = max compression
                byte[] buffer = new byte[4096];
                foreach (DtoDocument file in files)
                {
                    string filePath = Path.Combine(adhBasePath, file.filename);
                    ZipEntry entry = new ZipEntry(file.customName)
                    {
                        DateTime = DateTime.Now,
                    };
                    zipStream.PutNextEntry(entry);
                    using (FileStream fs = File.OpenRead(filePath))
                    {
                        // Using a fixed size buffer here makes no noticeable difference for output
                        // but keeps a lid on memory usage.
                        int sourceBytes;
                        do
                        {
                            sourceBytes = fs.Read(buffer, 0, buffer.Length);
                            zipStream.Write(buffer, 0, sourceBytes);
                        } while (sourceBytes > 0);
                    }
                }

                zipStream.Finish();

                // Close is important to wrap things up and unlock the file.
                zipStream.Close();
            }

            return zipName;
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

        public async Task<FileModel> GetFile(string filename, string uid)
        {
            var config = InitAdherentPaths(uid, false);
            string path = Path.Combine(config.BasePath, filename);
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                throw new System.IO.FileNotFoundException(path);
            }
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(path, out contentType))
            {
                contentType = "application/octet-stream";
            }
            var bytes = await File.ReadAllBytesAsync(path);
            return new FileModel(Path.GetFileName(path), bytes, contentType);
        }

        public async Task<FileModel> GetFile(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                throw new System.IO.FileNotFoundException(path);
            }
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(path, out contentType))
            {
                contentType = "application/octet-stream";
            }
            var bytes = await File.ReadAllBytesAsync(path);
            return new FileModel(Path.GetFileName(path), bytes, contentType);
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

        public async Task<byte[]> CreateExcelFile<T>(List<T> list, string fileName, string sheetName)
        {
            var dataTable = ToDataTable(list);
            byte[] excelFileBytes;
            try
            {
                excelFileBytes = await _fileExport.CreateFileWithSheets(fileName + ".xlsx", new DataTable[] { dataTable }, new string[] { sheetName });
            }
            catch (Exception ex)
            {
                throw;
            }
            return excelFileBytes;
        }

        private DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    if (Props[i].PropertyType == typeof(List<string>))
                    {
                        List<string> vals = (List<string>)Props[i].GetValue(item, null);
                        values[i] = vals != null && vals.Any() ? string.Join(", ", vals) : "";
                    }
                    else
                    {
                        values[i] = Props[i].GetValue(item, null);
                    }
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public async Task<byte[]> CreateEmailFile(List<string> list, string fileName, string extension)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn(list.First(), typeof(string)));
            foreach (var item in list.Skip(1))
            {
                dataTable.Rows.Add(new string[] { item });
            }
            
            string result;
            byte[] fileBytes;
            try
            {
                var basePath = _hostingEnvironment.WebRootPath;
                var paths = _config.GetSection("Paths");
                var path = Path.Combine(basePath, paths.GetValue<string>("Export"), fileName);
                result = _fileExport.CreateFile(path, extension, dataTable);
                fileBytes = await ReadFileAsBytes(new FileInfo(result));
            }
            catch (Exception ex)
            {
                throw;
            }
            return fileBytes;
        }
    }
}
