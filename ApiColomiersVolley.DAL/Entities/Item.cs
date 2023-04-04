using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.DAL.Entities
{
    [Table("item")]
    public class Item
    {
        [Key]
        [Column("id")]
        public int IdItem { get; set; }
        [Column("title", TypeName = "varchar(250)")]
        public string? Title { get; set; }
        [Column("content", TypeName = "text")]
        public string? Content { get; set; }
        [Column("date")]
        public DateTime? Date { get; set; }
        [Column("modified")]
        public DateTime? Modified { get; set; }
        [Column("type", TypeName = "varchar(50)")]
        [Required]
        public string Type { get; set; }
        [Column("slug", TypeName = "varchar(800)")]
        public string? Slug { get; set; }
        [Column("description", TypeName = "varchar(800)")]
        public string? Description { get; set; }
        [Column("resume", TypeName = "varchar(800)")]
        public string? Resume { get; set; }
        [Column("ip", TypeName = "varchar(800)")]
        public string? Ip { get; set; }
        [Column("order")]
        public int? Order { get; set; }
        [Column("public")]
        public bool? Public { get; set; }
        [Column("author")]
        public int? Author { get; set; }
        [Column("idParent")]
        public int? IdParent { get; set; }
        [Column("idCategory")]
        public int? IdCategory { get; set; }
        [Column("idPost")]
        public int? IdPost { get; set; }
    }
}
