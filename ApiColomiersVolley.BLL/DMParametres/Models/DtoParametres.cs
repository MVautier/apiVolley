using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMParametres.Models
{
    public class DtoParametres
    {
        public int IdParametre { get; set; }
        public bool InscriptionOpened { get; set; }
        public bool Reinscription { get; set; }
        public string? InscriptionFilter { get; set; }
        public bool AdoOpened { get; set; }
        public bool LoisirOpened { get; set; }
        public bool CompetOpened { get; set; }
        public int NbAdoMax { get; set; }

        public int TarifLocal { get; set; }
        public int TarifExterior { get; set; }
        public int TarifMember { get; set; }
        public int TarifLoisir { get; set; }
        public int TarifLicense { get; set; }
        public int TarifAdo { get; set; }

        public string? SubHeader { get; set; }
        public string? Text1 { get; set; }
        public string? Text2 { get; set; }
        public string? Text3 { get; set; }
    }
}
