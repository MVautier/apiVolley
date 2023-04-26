using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.Core.Tools.Models
{
    public class FileModel
    {
        public FileModel(string name, byte[] bytes, string type)
        {
            Name = name;
            Content = bytes;
            Type = type;
        }

        public string Name { get; set; }
        public byte[] Content { get; set; }
        public string Type { get; set; }

    }
}
