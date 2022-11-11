using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.DAL.Entities
{
    [Table("token")]
    public class Token
    {
        [Key]
        [Column("id")]
        public int IdToken { get; set; }
        [Column("key", TypeName = "varchar(8000)")]
        [Required(AllowEmptyStrings = false)]
        public string Key { get; set; }
        [Column("beginDate")]
        public DateTime BeginDate { get; set; }
        [Column("endDate")]
        public DateTime EndDate { get; set; }
        [Column("idConnexion")]
        public int IdConnexion { get; set; }
    }
}
