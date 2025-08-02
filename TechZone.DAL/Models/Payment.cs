using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechZone.DAL.Enums.Payment;

namespace TechZone.DAL.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; }
        public PaymentStatus Status { get; set; }

        public int OrderHeaderId { get; set; }
        public virtual OrderHeader Order { get; set; }

        //To track the payment from StripeApi
        //To refund the payment 
        public string? PaymentIntentId { get; set; }

        public string? SessionId { get; set; }
    } 
}
