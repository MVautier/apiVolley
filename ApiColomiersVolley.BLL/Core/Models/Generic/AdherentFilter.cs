using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.Core.Models.Generic
{
    public class AdherentFilter
    {
        public bool? HasPaid { get; set; }
        public bool? HasPhoto { get; set; }
        public bool? HasLicence { get; set; }
        public int? IdSection { get; set; }
        public int? IdCategory { get; set; }
        public string? Team { get; set; }
        public DynamicFilter? DynamicFilter { get; set; }
        public DateRange? DateRange { get; set; }
    }
}
