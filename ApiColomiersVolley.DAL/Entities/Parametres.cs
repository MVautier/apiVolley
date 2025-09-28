using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.DAL.Entities
{
    [Table("parametres")]
    public class Parametres
    {
        [Key]
        [Column("id")]
        public int IdParametre { get; set; }
        [Column("inscriptionOpened")]
        public bool InscriptionOpened { get; set; }
        [Column("reinscription")]
        public bool Reinscription { get; set; }
        [Column("inscriptionFilter", TypeName = "varchar(500)")]
        public string? InscriptionFilter { get; set; }
        [Column("adoOpened")]
        public bool AdoOpened { get; set; }
        [Column("loisirOpened")]
        public bool LoisirOpened { get; set; }
        [Column("competOpened")]
        public bool CompetOpened { get; set; }
        [Column("nbAdoMax")]
        public int NbAdoMax { get; set; }

        [Column("tarifLocal")]
        public int TarifLocal { get; set; }
        [Column("tarifExterior")]
        public int TarifExterior { get; set; }
        [Column("tarifMember")]
        public int TarifMember { get; set; }
        [Column("tarifLoisir")]
        public int TarifLoisir { get; set; }
        [Column("tarifLicense")]
        public int TarifLicense { get; set; }
        [Column("tarifAdo")]
        public int TarifAdo { get; set; }

        [Column("subHeader", TypeName = "varchar(800)")]
        public string? SubHeader { get; set; }
        [Column("text1", TypeName = "varchar(1000)")]
        public string? Text1 { get; set; }
        [Column("text2", TypeName = "varchar(1000)")]
        public string? Text2 { get; set; }
        [Column("text3", TypeName = "varchar(1000)")]
        public string? Text3 { get; set; }
    }
}
