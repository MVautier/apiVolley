using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models = ApiColomiersVolley.BLL.DMAuthentication.Models;

namespace ApiColomiersVolley.DAL.Entities.Extensions
{
    internal static class ExtensionUser
    {
        internal static Models.DtoUser ToDtoUser(this User user)
        {
            if (user == null)
            {
                return null;
            }

            return new Models.DtoUser
            {
                IdUser = user.IdUser,
                Nom = user.Nom,
                Prenom = user.Prenom,
                Role = user.Role,
                Mail = user.Mail,
                Password = user.Password,
                CreationDate = user.CreationDate,
                UpdateDate = user.UpdateDate
            };
        }

        internal static List<Models.DtoUser> ToDtoUser(this List<User> users)
        {
            return users.Select(d => d.ToDtoUser()).ToList();
        }
    }
}
