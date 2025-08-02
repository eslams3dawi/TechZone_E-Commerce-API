using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechZone.DAL.Enums.Order;
using TechZone.DAL.Models;

namespace TechZone.BLL.DTOs.OrderDTOs
{
    public class OrderHeaderReadDTO
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? ShippingDate { get; set; } //Initialized by Admin 
        public OrderStatus OrderStatus { get; set; }
        public string StatusName => OrderStatus.ToString();
    }
}
