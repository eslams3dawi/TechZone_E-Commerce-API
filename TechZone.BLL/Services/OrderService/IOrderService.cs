using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechZone.BLL.DTOs.OrderDTOs;
using TechZone.BLL.Wrappers;
using TechZone.DAL.Enums.Order;

namespace TechZone.BLL.Services.OrderService
{  
    public interface IOrderService
    {
        public Task<int> CreateOrder(OrderAddDTO orderAddDTO);
        public Task CancelOrder(int orderId);
        public Task UpdateOrderStatus(int orderId, OrderStatus orderStatus);
        public Task UpdateShippingDate(int orderId, DateTime shippingDate);
        public Task<Result<IEnumerable<OrderHeaderReadDTO>>> GetOrders();
        public Task<Result<string>> GetOrderHeader(int orderId);
        public Task<Result<IEnumerable<OrderDetailReadDTO>>> GetOrderDetails(int orderHeaderId);
    }
}
