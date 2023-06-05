using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiColomiersVolley.DAL.Entities
{
    [Table("adherent")]
    public class Adherent
    {
        [Key]
        [Column("id")]
        public int IdAdherent { get; set; }
        [ForeignKey("Section")]
        [Column("id_section")]
        public int? IdSection { get; set; }
        [ForeignKey("Category")]
        [Column("id_categorie")]
        public int? IdCategory { get; set; }
        [Column("autorisation_sortie", TypeName = "varchar(200)")]
        public string? Authorization { get; set; }
        [Column("nom", TypeName = "varchar(100)")]
        public string LastName { get; set; }
        [Column("prenom", TypeName = "varchar(100)")]
        public string FirstName { get; set; }
        [Column("genre", TypeName = "varchar(1)")]
        public string Genre { get; set; }
        [Column("date_naissance")]
        public DateTime? BirthdayDate { get; set; }
        [Column("date_inscription")]
        public DateTime? InscriptionDate { get; set; }
        [Column("date_questionnaire")]
        public DateTime? HealthStatementDate { get; set; }
        [Column("date_certificat")]
        public DateTime? CertificateDate { get; set; }
        [Column("tel", TypeName = "varchar(20)")]
        public string Phone { get; set; }
        [Column("tel_parent", TypeName = "varchar(100)")]
        public string? ParentPhone { get; set; }
        [Column("adresse", TypeName = "varchar(120)")]
        public string Address { get; set; }
        [Column("cp", TypeName = "varchar(5)")]
        public string PostalCode { get; set; }
        [Column("ville", TypeName = "varchar(100)")]
        public string City { get; set; }
        [Column("email", TypeName = "varchar(150)")]
        public string Email { get; set; }
        [Column("paiement", TypeName = "varchar(25)")]
        public string? Payment { get; set; }
        [Column("photo", TypeName = "varchar(100)")]
        public string? Photo { get; set; }
        [Column("equipe1", TypeName = "varchar(20)")]
        public string? Team1 { get; set; }
        [Column("equipe2", TypeName = "varchar(20)")]
        public string? Team2 { get; set; }
        [Column("licence", TypeName = "varchar(20)")]
        public string? Licence { get; set; }
        [Column("verif_paiement", TypeName = "varchar(100)")]
        public string? PaymentComment { get; set; }
        [Column("alert1", TypeName = "varchar(500)")]
        public string? Alert1 { get; set; }
        [Column("alert2", TypeName = "varchar(500)")]
        public string? Alert2 { get; set; }
        [Column("alert3", TypeName = "varchar(500)")]
        public string? Alert3 { get; set; }

        [Column("saison")]
        public int? Saison { get; set; }
        [Column("uid", TypeName = "varchar(36)")]
        public string? Uid { get; set; }
        [Column("sections", TypeName = "varchar(400)")]
        public string? Sections { get; set; }
        [Column("verif_c3l", TypeName = "varchar(500)")]
        public string? VerifC3L { get; set; }
        [Column("rgpd")]
        public bool? Rgpd { get; set; }
        [Column("image_right")]
        public bool? ImageRight { get; set; }
        [Column("signature", TypeName = "varchar(100)")]
        public string? Signature { get; set; }

        public virtual Section Section { get; set; }
        public virtual Category Category { get; set; }
    }
}
