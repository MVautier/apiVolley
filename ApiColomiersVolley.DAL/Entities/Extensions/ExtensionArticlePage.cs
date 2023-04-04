using ApiColomiersVolley.BLL.DMItem.Models;
using Models = ApiColomiersVolley.BLL.DMItem.Models;

namespace ApiColomiersVolley.DAL.Entities.Extensions
{
    internal static class ExtensionArticlePage
    {
        internal static DtoArticlePage ToDtoArticlePage(this ArticlePage item)
        {
            if (item == null)
            {
                return null;
            }

            return new DtoArticlePage
            {
                IdArticlePage = item.IdArticlePage,
                IdArticle = item.IdArticle,
                IdPage = item.IdPage
            };
        }

        internal static List<DtoArticlePage> ToDtoArticlePage(this List<ArticlePage> items)
        {
            return items.Select(d => d.ToDtoArticlePage()).ToList();
        }

        internal static ArticlePage ToArticlePage(this DtoArticlePage item)
        {
            if (item == null)
            {
                return null;
            }

            return new ArticlePage
            {
                IdArticlePage = item.IdArticlePage,
                IdArticle = item.IdArticle,
                IdPage = item.IdPage
            };
        }

        internal static List<ArticlePage> ToArticlePage(this List<DtoArticlePage> items)
        {
            return items.Select(d => d.ToArticlePage()).ToList();
        }
    }
}
