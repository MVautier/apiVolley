using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMAdherent.Models
{
    public class DtoAdherent
    {
        public int IdAdherent { get; set; }
        public string? Section { get; set; } // filled on insert/update
        public string Category { get; set; } // filled on insert/update
        public string? Authorization { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Genre { get; set; }
        public DateTime? BirthdayDate { get; set; }
        public DateTime? InscriptionDate { get; set; } // filled on insert/update
        public int? Age { get; set; } // filled on insert/update
        public DateTime? HealthStatementDate { get; set; }
        public string Phone { get; set; }
        public string? ParentPhone { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string? Photo { get; set; }
        public string? Licence { get; set; }
        public List<DtoAdherent> Membres { get; set; }
        public string? AlertLastName { get; set; }
        public string? AlertFirstName { get; set; }
        public string? AlertPhone { get; set; }
        public string? Uid { get; set; }
        public List<string>? Sections { get; set; }
        public bool? Rgpd { get; set; }
        public bool? ImageRight { get; set; }
        public string? Signature { get; set; }

        // Champs saisis par l'admin
        public int? Saison { get; set; }
        public string? Team1 { get; set; }
        public string? Team2 { get; set; }
        public string? PaymentComment { get; set; }
        public DateTime? CertificateDate { get; set; }
        public string? Payment { get; set; }
    }
}
