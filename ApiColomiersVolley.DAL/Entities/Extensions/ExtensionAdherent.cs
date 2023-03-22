using ApiColomiersVolley.BLL.DMAdherent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.DAL.Entities.Extensions
{
    internal static class ExtensionsAdherent
    {
        internal static DtoAdherent ToDtoAdherent(this Adherent adherent)
        {
            if (adherent == null)
            {
                return null;
            }

            return new DtoAdherent
            {
                IdAdherent = adherent.IdAdherent,
                Section = adherent.Section?.Libelle,
                Category = adherent.Category.Code,
                Authorization = adherent.Authorization,
                FirstName = adherent.FirstName,
                LastName = adherent.LastName,
                Genre = adherent.Genre,
                BirthdayDate = adherent.BirthdayDate,
                InscriptionDate= adherent.InscriptionDate,
                Age = GetAge(adherent.BirthdayDate),
                HealthStatementDate = adherent.HealthStatementDate,
                CertificateDate = adherent.CertificateDate,
                Phone = adherent.Phone,
                ParentPhone = adherent.ParentPhone,
                Address = adherent.Address,
                PostalCode = adherent.PostalCode,
                City = adherent.City,
                Email = adherent.Email,
                Payment = adherent.Payment,
                Photo = adherent.Photo,
                Team1 = adherent.Team1,
                Team2 = adherent.Team2,
                Licence = adherent.Licence,
                PaymentComment = adherent.PaymentComment,
                MainSectionInfo = adherent.MainSectionInfo,
                AlertLastName= adherent.AlertLastName,
                AlertFirstName = adherent.AlertFirstName,
                AlertPhone = adherent.AlertPhone
            };
        }

        internal static IEnumerable<DtoAdherent> ToDtoAdherent(this IEnumerable<Adherent> adherents)
        {
            return adherents.Select(a => a.ToDtoAdherent());
        }

        private static int? GetAge(DateTime? birthDate)
        {
            if (birthDate.HasValue)
            {
                return DateTime.Now.Year - birthDate.Value.Year;
            }

            return null;
        }
    }
}
