using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.Core.Models.Generic
{
    public class OrderFilter
    {
        public DateTime? start {  get; set; }
        public DateTime? end { get; set; }
        public int? season { get; set; }
    }
}
