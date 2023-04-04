using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models = ApiColomiersVolley.BLL.DMArticle.Models;

namespace ApiColomiersVolley.DAL.Entities.Extensions
{
    internal static class ExtensionArticle
    {
        internal static Models.DtoArticle ToDtoArticle(this Article article)
        {
            if (article == null)
            {
                return null;
            }

            return new Models.DtoArticle
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                Author = article.Author,
                Date = article.Date
            };
        }

        internal static List<Models.DtoArticle> ToDtoArticle(this List<Article> articles)
        {
            return articles.Select(d => d.ToDtoArticle()).ToList();
        }
    }
}
