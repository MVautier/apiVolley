using ApiColomiersVolley.BLL.DMAdherent.Models;
using ICSharpCode.SharpZipLib.Checksum;
using NPOI.POIFS.Properties;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static NPOI.HSSF.Util.HSSFColor;

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
                IdParent = adherent.IdParent,
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
                Alert1 = adherent.Alert1,
                Alert2 = adherent.Alert2,
                Alert3 = adherent.Alert3,
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
                IdParent = adherent.IdParent,
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
                Alert1 = adherent.Alert1,
                Alert2 = adherent.Alert2,
                Alert3 = adherent.Alert3,
                Saison = adherent.Saison,
                Uid = adherent.Uid,
                Sections = adherent.Sections != null && adherent.Sections.Any() ? string.Join(",", adherent.Sections) : null,
                Rgpd = adherent.Rgpd,
                ImageRight = adherent.ImageRight,
                Signature = adherent.Signature
            };
        }

        internal static Adherent ToAdherent(this DtoAdherent adherent, Adherent source)
        {
            if (adherent == null)
            {
                return null;
            }

            source.IdAdherent = adherent.IdAdherent;
            source.IdParent = adherent.IdParent;
            source.IdSection = GetIdSection(adherent.BirthdayDate);
            source.IdCategory = GetIdCategory(adherent.Category);
            source.Authorization = adherent.Authorization;
            source.FirstName = adherent.FirstName;
            source.LastName = adherent.LastName;
            source.Genre = adherent.Genre;
            source.BirthdayDate = adherent.BirthdayDate;
            source.InscriptionDate = adherent.InscriptionDate;
            source.CertificateDate= adherent.CertificateDate;
            source.HealthStatementDate = adherent.HealthStatementDate;
            source.Phone = adherent.Phone;
            source.ParentPhone = adherent.ParentPhone;
            source.Address = adherent.Address;
            source.PostalCode = adherent.PostalCode;
            source.City = adherent.City;
            source.Email = adherent.Email;
            source.Payment = adherent.Payment;
            source.PaymentComment = adherent.PaymentComment;
            source.Photo = adherent.Photo;
            if (!string.IsNullOrEmpty(adherent.Team1))
            {
                source.Team1 = adherent.Team1;
            }
            if (!string.IsNullOrEmpty(adherent.Team2))
            {
                source.Team2 = adherent.Team2;
            }
            if (!string.IsNullOrEmpty(adherent.Licence))
            {
                source.Licence = adherent.Licence;
            }
            source.Alert1 = adherent.Alert1;
            source.Alert2 = adherent.Alert2;
            source.Alert3 = adherent.Alert3;
            source.Saison = adherent.Saison;
            source.Uid = adherent.Uid;
            source.Sections = adherent.Sections != null && adherent.Sections.Any() ? string.Join(",", adherent.Sections) : null;
            source.Rgpd = adherent.Rgpd;
            source.ImageRight = adherent.ImageRight;
            source.Signature = adherent.Signature;

            return source;
        }

        internal static Adherent ToAdherentAdd(this DtoAdherent adherent)
        {
            if (adherent == null)
            {
                return null;
            }

            return new Adherent
            {
                IdParent= adherent.IdParent,
                IdSection = GetIdSection(adherent.BirthdayDate),
                IdCategory = GetIdCategory(adherent.Category),
                Authorization = adherent.Authorization,
                FirstName = adherent.FirstName,
                LastName = adherent.LastName,
                Genre = adherent.Genre,
                BirthdayDate = adherent.BirthdayDate,
                InscriptionDate = DateTime.Now,
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
                Alert1 = adherent.Alert1,
                Alert2 = adherent.Alert2,
                Alert3 = adherent.Alert3,
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
