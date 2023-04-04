using ApiColomiersVolley.BLL.DMItem.Models;
using Models = ApiColomiersVolley.BLL.DMItem.Models;

namespace ApiColomiersVolley.DAL.Entities.Extensions
{
    internal static class ExtensionItem
    {
        internal static Models.WebItem ToWebItem(this Item item, List<User> users)
        {
            if (item == null)
            {
                return null;
            }

            var user = item.Author.HasValue ? users.FirstOrDefault(u => u.IdUser == item.Author.Value) : null;

            return new Models.WebItem
            {
                IdItem = item.IdItem,
                Title = item.Title,
                Content = item.Content,
                Date = item.Date,
                Modified = item.Modified,
                Type = item.Type,
                Slug = item.Slug,
                Description = item.Description,
                Order = item.Order,
                Public = item.Public,
                Resume = item.Resume,
                IdAuthor = item.Author,
                Author = user != null ? user.Prenom + " " + user.Nom : String.Empty,
                IdParent = item.IdParent,
                IdCategory = item.IdCategory,
                IdPost = item.IdPost
            };
        }

        internal static List<Models.WebItem> ToWebItem(this List<Item> items, List<User> users)
        {
            return items.Select(d => d.ToWebItem(users)).ToList();
        }

        internal static Item ToItem(this WebItem item)
        {
            if (item == null)
            {
                return null;
            }

            return new Item
            {
                IdItem = item.IdItem,
                Title = item.Title,
                Content = item.Content,
                Date = item.Date,
                Modified = item.Modified,
                Type = item.Type,
                Slug = item.Slug,
                Description = item.Description,
                Order = item.Order,
                Public = item.Public,
                Resume = item.Resume,
                Author = item.IdAuthor,
                IdParent = item.IdParent,
                IdCategory = item.IdCategory,
                IdPost = item.IdPost
            };
        }

        internal static List<Item> ToItem(this List<WebItem> items)
        {
            return items.Select(d => d.ToItem()).ToList();
        }
    }
}
