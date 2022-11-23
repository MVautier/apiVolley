using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMItem.Models
{
    public class DtoArticlePage
    {
        public int IdArticlePage { get; set; }
        public int IdArticle { get; set; }
        public int IdPage { get; set; }
    }
}
