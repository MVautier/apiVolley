using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMAdherent.Models
{
    public class DtoOrderExport : DtoOrder
    {
        public string? PaymentComment { get; set; }
        public string? Members { get; set; }
    }
}
