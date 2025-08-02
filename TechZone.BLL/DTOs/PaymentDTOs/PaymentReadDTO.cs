using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechZone.DAL.Enums.Payment;
using TechZone.DAL.Models;

namespace TechZone.BLL.DTOs.PaymentDTOs
{
    public class PaymentReadDTO
    {
        public int PaymentId { get; set; }
        public int OrderHeaderId { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? SessionId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; }
        public PaymentStatus Status { get; set; }
    }
}
