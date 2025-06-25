using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.Core.Helloasso.Models
{
    public class CartItem
    {
        public string libelle { get; set; }
        public int montant { get; set; }
        public string type { get; set; }
        public string[] user { get; set; }
    }
}
