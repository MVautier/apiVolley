using ApiColomiersVolley.BLL.DMAdherent.Models;
using ApiColomiersVolley.BLL.DMUser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.DAL.Entities.Extensions
{
    internal static class ExtensionRole
    {
        internal static DtoRole ToDtoRole(this Role role)
        {
            if (role == null)
            {
                return null;
            }

            return new DtoRole
            {
                Id = role.IdRole,
                Libelle = role.Libelle
            };
        }

        internal static IEnumerable<DtoRole> ToDtoRole(this IEnumerable<Role> roles)
        {
            return roles.Select(d => d.ToDtoRole()).ToList();
        }
    }
}
