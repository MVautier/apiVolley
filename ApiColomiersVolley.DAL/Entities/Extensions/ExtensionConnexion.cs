using ApiColomiersVolley.BLL.DMAuthentication.Models;
using ApiColomiersVolley.BLL.DMItem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models = ApiColomiersVolley.BLL.DMAuthentication.Models;

namespace ApiColomiersVolley.DAL.Entities.Extensions
{
    internal static class ExtensionConnexion
    {
        internal static Models.DtoConnexion ToDtoConnexion(this Connexion conn)
        {
            if (conn == null)
            {
                return null;
            }

            return new Models.DtoConnexion
            {
                IdConnexion = conn.IdConnexion,
                Login = conn.Login,
                BeginDate = conn.BeginDate,
                LastRefresh = conn.LastRefresh,
                EndDate = conn.EndDate,
                Ip = conn.Ip,
                RefreshCount = conn.RefreshCount,
                IdUser = conn.IdUser
            };
        }

        internal static List<Models.DtoConnexion> ToDtoConnexion(this List<Connexion> conns)
        {
            return conns.Select(d => d.ToDtoConnexion()).ToList();
        }

        internal static Connexion ToConnexion(this DtoConnexion conn)
        {
            if (conn == null)
            {
                return null;
            }

            return new Connexion
            {
                IdConnexion = conn.IdConnexion,
                Login = conn.Login,
                BeginDate = conn.BeginDate,
                LastRefresh = conn.LastRefresh,
                EndDate = conn.EndDate,
                Ip = conn.Ip,
                RefreshCount = conn.RefreshCount,
                IdUser = conn.IdUser
            };
        }

        internal static List<Connexion> ToConnexion(this List<DtoConnexion> conns)
        {
            return conns.Select(d => d.ToConnexion()).ToList();
        }
    }
}
