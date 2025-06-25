using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.Core.Helloasso.Models
{
    public class PaymentRequest
    {
        public int totalAmount { get; set; }
        public int initialAmount { get; set; }
        public string itemName { get; set; }
        public string backUrl { get; set; }
        public string errorUrl { get; set; }
        public string returnUrl { get; set; }
        public bool containsDonation { get; set; }
        public Term[] terms { get; set; }
        public Payer payer { get; set; }
        public object metadata { get; set; }
        public string trackingParameter { get; set; }

        public PaymentRequest(Cart cart, string itemName, string basePath)
        {
            this.totalAmount = cart.total * 100;
            this.initialAmount = cart.total * 100;
            this.itemName = itemName;
            this.backUrl = basePath + "inscription?step=4&payment=cancel";
            this.errorUrl = basePath + "inscription?step=4&payment=error";
            this.returnUrl = basePath + "inscription?step=4&payment=success";
            this.containsDonation = false;
            this.payer = new Payer
            {
                firstName = cart.client.FirstName,
                lastName = cart.client.LastName,
                email = cart.client.Email,
                dateOfBirth = cart.client.BirthdayDate,
                address = cart.client.Address,
                city = cart.client.City,
                zipCode = cart.client.PostalCode,
                country = "FRA",
            };
            this.metadata = cart;
        }
    }
}
