using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMArticle.Models
{
    public class DtoArticle
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Author { get; set; }
        public DateTime Date { get; set; }
    }
}
