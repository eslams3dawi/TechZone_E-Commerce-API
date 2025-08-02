using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe.Climate;
using TechZone.BLL.DTOs.PaymentDTOs;
using TechZone.BLL.Services.OrderService;
using TechZone.BLL.Services.PaymentService;
using TechZone.DAL.Enums.Payment;
using TechZone.DAL.Models;

namespace TechZone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;

        public PaymentsController(IPaymentService paymentService, IOrderService orderService)
        {
            _paymentService = paymentService;
            _orderService = orderService;
        }
        [Authorize(Roles = "Admin, User")]
        [HttpPost("create-checkout-session/{orderId}")]
        public async Task<ActionResult> CreateCheckoutSession(int orderId)
        {
            var orderDetails = await _orderService.GetOrderDetails(orderId);

            var checkoutItems = orderDetails.Data.Select(od => new CheckoutProductDTO
            {
                Name = od.ProductName,
                Price = (long)(od.Price * 100), //from dollars to cents
                Quantity = od.Count
            }).ToList();

            var url = await _paymentService.CreateCheckoutSession(checkoutItems, orderId);

            return Ok(new { checkoutUrl = url });
        }

        [HttpGet("success")]
        public async Task<ActionResult> Success()
        {
            return Ok("Payment Successful!");
        }

        [HttpGet("cancel")]
        public async Task<ActionResult> Cancel()
        {
            return Ok("Payment Cancelled");
        }
    }
}
