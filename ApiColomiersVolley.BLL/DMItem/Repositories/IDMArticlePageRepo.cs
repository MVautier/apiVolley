using ApiColomiersVolley.BLL.DMItem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMItem.Repositories
{
    public interface IDMArticlePageRepo
    {
        Task<List<DtoArticlePage>> GetByIdArticle(int idArticle);
        Task<DtoArticlePage> AddOrUpdate(DtoArticlePage item);
        Task<bool> Remove(int id);
    }
}
