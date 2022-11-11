using ApiColomiersVolley.BLL.DMItem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models = ApiColomiersVolley.BLL.DMItem.Models;

namespace ApiColomiersVolley.DAL.Entities.Extensions
{
    internal static class ExtensionItem
    {
        internal static Models.DtoItem ToDtoItem(this Item item)
        {
            if (item == null)
            {
                return null;
            }

            return new Models.DtoItem
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
                Author = item.Author,
                IdParent = item.IdParent,
                IdCategory = item.IdCategory,
                IdPost = item.IdPost
            };
        }

        internal static List<Models.DtoItem> ToDtoItem(this List<Item> items)
        {
            return items.Select(d => d.ToDtoItem()).ToList();
        }

        internal static Item ToItem(this DtoItem item)
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
                Author = item.Author,
                IdParent = item.IdParent,
                IdCategory = item.IdCategory,
                IdPost = item.IdPost
            };
        }

        internal static List<Item> ToItem(this List<DtoItem> items)
        {
            return items.Select(d => d.ToItem()).ToList();
        }
    }
}
