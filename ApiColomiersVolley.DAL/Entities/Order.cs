using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.DAL.Entities
{
    [Table("commande")]
    public class Order
    {
        [Key]
        [Column("id")]
        public int IdOrder { get; set; }
        [Column("id_paiement")]
        public int IdPaiement { get; set; }
        [Column("id_adherent")]
        public int IdAdherent { get; set; }
        [Column("date")]
        public DateTime? Date { get; set; }
        [Column("cotisation_c3l")]
        public int? CotisationC3L { get; set; }
        [Column("total")]
        public int? Total { get; set; }
        [Column("nom")]
        public string? Nom { get; set; }
        [Column("prennom")]
        public string? Prenom { get; set; }
        [Column("email")]
        public string? Email { get; set; }
        [Column("date_naissance")]
        public DateTime? DateNaissance { get; set; }
    }
}
