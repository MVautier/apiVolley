using ApiColomiersVolley.BLL.DMAdherent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.DAL.Entities.Extensions
{
    internal static class ExtensionCategory
    {
        internal static DtoCategory ToDtoCategory(this Category categ)
        {
            if (categ == null)
            {
                return null;
            }

            return new DtoCategory
            {
                IdCategory = categ.IdCategory,
                Code = categ.Code,
                Libelle = categ.Libelle
            };
        }

        internal static IEnumerable<DtoCategory> ToDtoCategory(this IEnumerable<Category> categs)
        {
            return categs.Select(d => d.ToDtoCategory()).ToList();
        }
    }
}
