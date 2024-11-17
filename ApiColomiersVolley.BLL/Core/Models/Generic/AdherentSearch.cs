using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.Core.Models.Generic
{
    public class AdherentSearch
    {
        public string nom { get; set; }
        public string prenom { get; set; }
        public DateTime? birthdayDate { get; set; }
    }
}
