using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechZone.BLL.DTOs.OrderDTOs;
using TechZone.BLL.DTOs.PaymentDTOs;
using TechZone.BLL.Services.OrderService;
using TechZone.BLL.Wrappers;
using TechZone.DAL.Enums.Order;

namespace TechZone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost]
        public async Task<ActionResult> CreateOrder(OrderAddDTO orderAddDTO)
        {
            var orderId = await _orderService.CreateOrder(orderAddDTO);

            return CreatedAtAction(nameof(GetOrderDetails), new { orderHeaderId = orderId}, new { Message = "Order created successfully and its status is pending"});
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        public async Task<ActionResult<Result<IEnumerable<OrderHeaderReadDTO>>>> GetAllOrders()
        {
            var orders = await _orderService.GetOrders();

            return Ok(orders);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpDelete("{orderId}")]
        public async Task<ActionResult> CancelOrder(int orderId)
        {
            await _orderService.CancelOrder(orderId);

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{orderId}/status/{orderStatus}")]
        public async Task<ActionResult> UpdateOrderStatus(int orderId, OrderStatus orderStatus)
        {
            await _orderService.UpdateOrderStatus(orderId, orderStatus);

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{orderId}/shippingDate/{dateTime}")]
        public async Task<ActionResult> UpdateShippingDate(int orderId, DateTime dateTime)
        {
            await _orderService.UpdateShippingDate(orderId, dateTime);
            
            return NoContent();
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("orders/{orderHeaderId}/details")]
        public async Task<ActionResult<Result<IEnumerable<OrderDetailReadDTO>>>> GetOrderDetails(int orderHeaderId)
        {
            var result = await _orderService.GetOrderDetails(orderHeaderId);

            return Ok(result);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("{orderId}/status")]
        public async Task<ActionResult<Result<string>>> GetOrderStatus(int orderId)
        {
            var status = await _orderService.GetOrderHeader(orderId);

            return Ok(status);
        }
    }
}
