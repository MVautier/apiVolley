using ApiColomiersVolley.BLL.DMAuthentication.Models;
using ApiColomiersVolley.BLL.DMUser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.DAL.Entities.Extensions
{
    internal static class ExtensionUser
    {
        internal static DtoUser ToDtoUser(this User user)
        {
            if (user == null)
            {
                return null;
            }

            return new DtoUser
            {
                IdUser = user.IdUser,
                Nom = user.Nom,
                Prenom = user.Prenom,
                Role = user.Role.Libelle,
                Mail = user.Mail,
                Password = user.Password,
                CreationDate = user.CreationDate,
                UpdateDate = user.UpdateDate,
                EndDate = user.EndDate
            };
        }

        internal static List<DtoUser> ToDtoUser(this List<User> users)
        {
            return users.Select(d => d.ToDtoUser()).ToList();
        }

        internal static DtoUserRole ToDtoUserRole(this User user)
        {
            if (user == null)
            {
                return null;
            }

            return new DtoUserRole
            {
                IdUser = user.IdUser,
                Nom = user.Nom,
                Prenom = user.Prenom,
                Role = user.Role.Libelle,
                Mail = user.Mail,
                Password = user.Password,
                CreationDate = user.CreationDate,
                UpdateDate = user.UpdateDate,
                EndDate = user.EndDate,
                IdRole = user.IdRole
            };
        }

        internal static List<DtoUserRole> ToDtoUserRole(this List<User> users)
        {
            return users.Select(d => d.ToDtoUserRole()).ToList();
        }

        internal static UserInfo ToUserInfo(this User user)
        {
            if (user == null)
            {
                return null;
            }

            return new UserInfo
            {
                IdUser = user.IdUser,
                LastName = user.Nom,
                FirstName = user.Prenom,
                Mail = user.Mail,
                ExpireDate = user.EndDate,
                Role = user.Role.Libelle,
            };
        }
    }
}
