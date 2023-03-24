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
        Task<string> SavePdf(string filename, string id, IFormFile file);
    }
}
