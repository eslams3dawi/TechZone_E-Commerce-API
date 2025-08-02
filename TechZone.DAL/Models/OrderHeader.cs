using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechZone.DAL.Enums.Order;

namespace TechZone.DAL.Models
{
    //for User
    //Create Order - generic
    //Delete Order (when cancellation) - generic

    //for Admin
    //Edit OrderStatus - generic
    //Initialize ShippingDate - generic
    //Get All Orders - generic → OrderHeaderReadDTO
    //Get Order details - generic → OrderDetailsReadDTO
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
        //public decimal TotalPrice { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
