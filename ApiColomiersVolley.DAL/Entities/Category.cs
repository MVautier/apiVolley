using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.DAL.Entities
{
    [Table("categorie")]
    public class Category
    {
        [Key]
        [Column("id")]
        public int IdCategory { get; set; }
        [Column("code", TypeName = "varchar(1)")]
        public string Code { get; set; }
        [Column("libelle", TypeName = "varchar(50)")]
        public string Libelle { get; set; }
    }
}
