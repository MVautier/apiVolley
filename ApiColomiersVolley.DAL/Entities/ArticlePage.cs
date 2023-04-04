using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.DAL.Entities
{
    [Table("article_page")]
    public class ArticlePage
    {
        [Key]
        [Column("id")]
        public int IdArticlePage { get; set; }
        [Column("idpost")]
        public int IdArticle { get; set; }
        [Column("idpage")]
        public int IdPage { get; set; }
    }
}
