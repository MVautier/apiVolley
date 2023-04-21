using ApiColomiersVolley.BLL.DMAdherent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
                InscriptionDate = adherent.InscriptionDate,
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
                PaymentComment = adherent.PaymentComment,
                Photo = adherent.Photo,
                Team1 = adherent.Team1,
                Team2 = adherent.Team2,
                Licence = adherent.Licence,
                AlertLastName = adherent.AlertLastName,
                AlertFirstName = adherent.AlertFirstName,
                AlertPhone = adherent.AlertPhone,
                Saison = adherent.Saison,
                Uid = adherent.Uid,
                Sections = adherent.Sections != null ? adherent.Sections.Split(',').ToList() : null,
                Rgpd = adherent.Rgpd,
                ImageRight = adherent.ImageRight,
                Signature = adherent.Signature
            };
        }

        internal static IEnumerable<DtoAdherent> ToDtoAdherent(this IEnumerable<Adherent> adherents)
        {
            return adherents.Select(a => a.ToDtoAdherent());
        }

        internal static Adherent ToAdherent(this DtoAdherent adherent)
        {
            if (adherent == null)
            {
                return null;
            }

            return new Adherent
            {
                IdAdherent = adherent.IdAdherent,
                IdSection = GetIdSection(adherent.BirthdayDate),
                IdCategory = GetIdCategory(adherent.Category),
                Authorization = adherent.Authorization,
                FirstName = adherent.FirstName,
                LastName = adherent.LastName,
                Genre = adherent.Genre,
                BirthdayDate = adherent.BirthdayDate,
                InscriptionDate = adherent.InscriptionDate,
                HealthStatementDate = adherent.HealthStatementDate,
                CertificateDate = adherent.CertificateDate,
                Phone = adherent.Phone,
                ParentPhone = adherent.ParentPhone,
                Address = adherent.Address,
                PostalCode = adherent.PostalCode,
                City = adherent.City,
                Email = adherent.Email,
                Payment = adherent.Payment,
                PaymentComment = adherent.PaymentComment,
                Photo = adherent.Photo,
                Team1 = adherent.Team1,
                Team2 = adherent.Team2,
                Licence = adherent.Licence,
                AlertLastName = adherent.AlertLastName,
                AlertFirstName = adherent.AlertFirstName,
                AlertPhone = adherent.AlertPhone,
                Saison = adherent.Saison,
                Uid = adherent.Uid,
                Sections = adherent.Sections != null && adherent.Sections.Any() ? string.Join(",", adherent.Sections) : null,
                Rgpd = adherent.Rgpd,
                ImageRight = adherent.ImageRight,
                Signature = adherent.Signature
            };
        }

        internal static IEnumerable<Adherent> ToAdherent(this IEnumerable<DtoAdherent> adherents)
        {
            return adherents.Select(a => a.ToAdherent());
        }

        private static int? GetIdSection(DateTime? birthDate)
        {
            int? age = GetAge(birthDate);
            if (age.HasValue)
            {
                return age <= 16 ? 1 : (age <= 18 ? 2 : 3);
            }

            return null;
        }

        private static int? GetIdCategory(string categ)
        {
            return categ == "C" ? 1 : (categ == "L" ? 2 : (categ == "E" ? 3 : null));
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
