using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.Core.Models.Generic
{
    public class OrderFilter
    {
        public DateOnly? start {  get; set; }
        public DateOnly? end { get; set; }
        public int? season { get; set; }
    }
}
