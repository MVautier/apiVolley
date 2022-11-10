using ApiColomiersVolley.BLL.DMArticle.Business.Interfaces;
using ApiColomiersVolley.BLL.DMArticle.Models;
using ApiColomiersVolley.BLL.DMArticle.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMArticle.Business
{
    public class BSArticle : IBSArticle
    {
        private readonly IConfiguration _config;
        private readonly IDMArticleRepo _articleRepo;

        public BSArticle(IConfiguration config, IDMArticleRepo articleRepo)
        {
            _config = config;
            _articleRepo = articleRepo;
        }

        public async Task<List<DtoArticle>> GetArticles()
        {
            return await _articleRepo.Get();
        }
    }
}
