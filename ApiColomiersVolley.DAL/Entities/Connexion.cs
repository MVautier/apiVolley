using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.DAL.Entities
{
    [Table("connexion")]
    public class Connexion
    {
        [Key]
        [Column("id")]
        public int IdConnexion { get; set; }
        [Column("login", TypeName = "varchar(250)")]
        public string? Login { get; set; }
        [Column("beginDate")]
        public DateTime BeginDate { get; set; }
        [Column("lastRefresh")]
        public DateTime LastRefresh { get; set; }
        [Column("endDate")]
        public DateTime? EndDate { get; set; }
        [Column("ip", TypeName = "varchar(50)")]
        public string Ip { get; set; }
        [Column("refreshCount")]
        public int RefreshCount { get; set; }
        [Column("idUser")]
        public int? IdUser { get; set; }
    }
}
