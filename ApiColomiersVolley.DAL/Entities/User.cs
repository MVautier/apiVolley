using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiColomiersVolley.DAL.Entities
{
    [Table("user")]
    public class User
    {
        [Key]
        [Column("id")]
        public int IdUser { get; set; }
        [Column("nom", TypeName = "varchar(250)")]
        public string? Nom { get; set; }
        [Column("prenom", TypeName = "varchar(250)")]
        public string? Prenom { get; set; }
        [Column("mail", TypeName = "varchar(250)")]
        public string? Mail { get; set; }
        [Column("password", TypeName = "varchar(250)")]
        public string? Password { get; set; }
        [Column("creationDate")]
        public DateTime? CreationDate { get; set; }
        [Column("creationDate")]
        public DateTime? UpdateDate { get; set; }
        [Column("role", TypeName = "varchar(50)")]
        [Required]
        public string Role { get; set; }
    }
}
