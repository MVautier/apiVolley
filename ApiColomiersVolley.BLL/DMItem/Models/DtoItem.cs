using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMItem.Models
{
    public class DtoItem
    {
        public int IdItem { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Modified { get; set; }
        public string Type { get; set; }
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public string? Resume { get; set; }
        public string? Ip { get; set; }
        public int? Order { get; set; }
        public bool? Public { get; set; }
        public int? Author { get; set; }
        public int? IdParent { get; set; }
        public int? IdCategory { get; set; }
        public int? IdPost { get; set; }
    }
}
