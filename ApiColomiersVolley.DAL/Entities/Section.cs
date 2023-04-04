using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.DAL.Entities
{
    [Table("section")]
    public class Section
    {
        [Key]
        [Column("id")]
        public int IdSection { get; set; }
        [Column("libelle", TypeName = "varchar(50)")]
        public string Libelle { get; set; }
    }
}
