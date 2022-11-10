using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiColomiersVolley.DAL.Entities
{
    [Table("article")]
    public class Article
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("title", TypeName = "varchar(250)")]
        public string? Title { get; set; }
        [Column("content", TypeName = "text")]
        public string? Content { get; set; }
        [Column("link", TypeName = "varchar(500)")]
        public string? Link { get; set; }
        [Column("author", TypeName = "varchar(250)")]
        public string? Author { get; set; }
        [Column("date")]
        public DateTime Date { get; set; }
    }
}
