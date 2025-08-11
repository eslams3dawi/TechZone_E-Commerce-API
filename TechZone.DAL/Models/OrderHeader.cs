using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechZone.DAL.Enums.Order;

namespace TechZone.DAL.Models
{
    public class OrderHeader
    {
        //Order
        public int Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public DateTime? ShippingDate { get; set; } //Initialized by Admin 
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
            //Edit by Admin
            //User can cancel only


        //MapFrom(List<ShoppingCartReadDTO> to List<OrderDetail> then fill it with ↓
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        //Payment
        public virtual Payment Payment { get; set; }

        //Customer
        public string ApplicationUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
