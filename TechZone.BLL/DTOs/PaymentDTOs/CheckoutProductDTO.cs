using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechZone.BLL.DTOs.PaymentDTOs
{
    public class CheckoutProductDTO
    {
        public string Name { get; set; }
        public long Price { get; set; }
        public int Quantity { get; set; }
    }
}
