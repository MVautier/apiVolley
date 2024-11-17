using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models = ApiColomiersVolley.BLL.DMAdherent.Models;

namespace ApiColomiersVolley.DAL.Entities.Extensions
{
    internal static class ExtensionOrder
    {
        internal static Models.DtoOrder ToDtoOrder(this Order order)
        {
            if (order == null)
            {
                return null;
            }

            return new Models.DtoOrder
            {
                Id = order.IdOrder,
                IdPaiement= order.IdPaiement,
                IdAdherent = order.IdAdherent,
                Saison = order.Saison,
                Date = order.Date,
                CotisationC3L = order.CotisationC3L,
                Total = order.Total,
                Nom = order.Nom,
                Prenom = order.Prenom,
                Email = order.Email,
                DateNaissance = order.DateNaissance,
                PaymentLink = order.PaymentLink
            };
        }

        internal static List<Models.DtoOrder> ToDtoOrder(this List<Order> orders)
        {
            return orders.Select(d => d.ToDtoOrder()).ToList();
        }

        internal static Order ToOrder(this Models.DtoOrder order)
        {
            if (order == null || !order.Id.HasValue || !order.IdPaiement.HasValue || !order.IdAdherent.HasValue || !order.Saison.HasValue)
            {
                return null;
            }

            return new Order
            {
                IdOrder = order.Id.Value,
                IdPaiement = order.IdPaiement.Value,
                IdAdherent = order.IdAdherent.Value,
                Saison = order.Saison.Value,
                Date = order.Date,
                CotisationC3L = order.CotisationC3L,
                Total = order.Total,
                Nom = order.Nom,
                Prenom = order.Prenom,
                Email = order.Email,
                DateNaissance = order.DateNaissance,
                PaymentLink = order.PaymentLink
            };
        }

        internal static List<Order> ToOrder(this List<Models.DtoOrder> orders)
        {
            return orders.Select(d => d.ToOrder()).ToList();
        }

        internal static Order ToOrderAdd(this Models.DtoOrder order)
        {
            if (order == null || !order.IdPaiement.HasValue || !order.IdAdherent.HasValue || !order.Saison.HasValue)
            {
                return null;
            }

            return new Order
            {
                IdPaiement = order.IdPaiement.Value,
                IdAdherent = order.IdAdherent.Value,
                Saison = order.Saison.Value,
                Date = order.Date,
                CotisationC3L = order.CotisationC3L,
                Total = order.Total,
                Nom = order.Nom,
                Prenom= order.Prenom,
                Email= order.Email,
                DateNaissance= order.DateNaissance,
                PaymentLink = order.PaymentLink
            };
        }

        internal static Order ToOrder(this Models.DtoOrder order, Order source)
        {
            if (order == null || !order.Id.HasValue || !order.IdPaiement.HasValue || !order.IdAdherent.HasValue || !order.Saison.HasValue)
            {
                return null;
            }

            source.IdOrder = order.Id.Value;
            source.IdPaiement = order.IdPaiement.Value;
            source.IdAdherent = order.IdAdherent.Value;
            source.Saison = order.Saison.Value;
            source.Date = order.Date;
            source.CotisationC3L = order.CotisationC3L; 
            source.Total = order.Total;
            source.Nom = order.Nom;
            source.Prenom = order.Prenom;
            source.Email = order.Email;
            source.DateNaissance = order.DateNaissance;
            source.PaymentLink = order.PaymentLink;

            return source;
        }
    }
}
