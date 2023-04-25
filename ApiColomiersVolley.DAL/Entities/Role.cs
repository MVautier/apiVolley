using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.DAL.Entities
{
    [Table("role")]
    public class Role
    {
        [Key]
        [Column("id")]
        public int IdRole { get; set; }
        [Column("libelle")]
        public string Libelle { get; set; }
    }
}
