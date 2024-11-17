using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMAdherent.Models
{
    public class DtoOrderFull : DtoOrder
    {
        public int? IdParent { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public DateTime? BirthdayDate { get; set; }
        public string? Payment { get; set; }
        public DateTime? InscriptionDate { get; set; }
        public List<DtoAdherent>? Membres { get; set; }
        public string PaymentMode { get; set; }

        public DtoOrderFull(DtoAdherent adherent, DtoOrder? order, IEnumerable<DtoAdherent> membres)
        {
            this.Id = order?.Id;
            this.IdPaiement = order?.IdPaiement;
            this.IdAdherent = order?.IdAdherent ?? adherent?.IdAdherent;
            this.Saison = order?.Saison ?? adherent?.Saison;
            this.Date = order?.Date;
            this.CotisationC3L = order?.CotisationC3L;
            this.Total = order?.Total;
            this.Nom = order?.Nom;
            this.Prenom= order?.Prenom;
            this.Email = order?.Email;
            this.DateNaissance = order?.DateNaissance;
            this.PaymentLink = order?.PaymentLink;
            this.IdParent = adherent?.IdParent;
            this.LastName = adherent?.LastName;
            this.FirstName = adherent?.FirstName;
            this.BirthdayDate = adherent?.BirthdayDate;
            this.Payment = adherent?.PaymentComment;
            this.InscriptionDate = adherent?.InscriptionDate;
            this.Membres = membres?.ToList() ?? null;
            this.PaymentMode = order != null ? "Helloasso" : "Manuel";
        }
    }
}
