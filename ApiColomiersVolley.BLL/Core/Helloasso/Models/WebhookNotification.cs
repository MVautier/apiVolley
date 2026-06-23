using System.Collections.Generic;

namespace ApiColomiersVolley.BLL.Core.Helloasso.Models
{
    // Structure observée sur un vrai paiement sandbox (cf. plan, Étape 0) :
    // { "eventType": "Order", "data": { "id", "checkoutIntentId", "payments": [...] }, "metadata": { ...Cart } }
    public class WebhookNotification
    {
        public string eventType { get; set; }
        public WebhookOrderData data { get; set; }
        public Cart metadata { get; set; }
    }

    public class WebhookOrderData
    {
        public long id { get; set; }
        public long checkoutIntentId { get; set; }
        public List<WebhookPayment> payments { get; set; }
    }

    public class WebhookPayment
    {
        public long id { get; set; }
        public string state { get; set; }
        public string paymentReceiptUrl { get; set; }
    }
}
