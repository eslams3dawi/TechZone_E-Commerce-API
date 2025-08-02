using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechZone.DAL.Enums.Payment;
using TechZone.DAL.Models;

namespace TechZone.BLL.DTOs.PaymentDTOs
{
    public class PaymentAddDTO
    {
        [Required]
        public int OrderHeaderId { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? SessionId { get; set; }
        [Required]
        public DateTime PaymentDate { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string Method { get; set; }
    }
}
