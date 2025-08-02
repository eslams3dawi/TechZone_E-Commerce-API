using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechZone.DAL.Enums.Order
{
    public enum OrderStatus
    {
        Pending, //Pending
        Approved, //Paid
        Shipped,
        Delivered, //Paid
        Cancelled //Refunded
    }
}
