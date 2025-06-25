using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.Core.Helloasso.Models
{
    public class Cart
    {
        public int id { get; set; }
        public CartItem[] items { get; set; }
        public DateTime date { get; set; }
        public int total { get; set; }
        public Client client { get; set; }
    }
}
