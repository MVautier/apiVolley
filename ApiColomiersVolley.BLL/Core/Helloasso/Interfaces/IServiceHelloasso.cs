using ApiColomiersVolley.BLL.Core.Helloasso.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.Core.Helloasso.Interfaces
{
    public interface IServiceHelloasso
    {
        Task<PostIntentResult> SendCheckout(Cart cart);
        Task<GetIntentResult> GetReceiptUrl(string id);
    }
}
