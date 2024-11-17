using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMAdherent.Models
{
    public class DtoOrder
    {
        public int? Id { get; set; }
        public int? IdPaiement { get; set; }
        public int? IdAdherent { get; set; }
        public int? Saison { get; set; }
        public DateTime? Date { get; set; }
        public int? CotisationC3L { get; set; }
        public int? Total { get; set; }
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
        public string? Email { get; set; }
        public DateTime? DateNaissance { get; set; }
        public string? PaymentLink { get; set; }
    }
}
