using ApiColomiersVolley.BLL.DMArticle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMArticle.Repositories
{
    public interface IDMArticleRepo
    {
        Task<List<DtoArticle>> Get();
    }
}
