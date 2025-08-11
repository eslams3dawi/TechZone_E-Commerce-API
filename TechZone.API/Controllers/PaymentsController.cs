using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechZone.BLL.Services.OrderService;
using TechZone.BLL.Services.PaymentService;
using Stripe.Checkout;
using TechZone.DAL.Enums.Payment;
using TechZone.DAL.Enums.Order;
using Microsoft.AspNetCore.Http.HttpResults;
using TechZone.API.Middleware.CustomExceptions;

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
            var url = await _paymentService.CreateCheckoutSession(orderId);

            return Ok(new { checkoutUrl = url });
        }

        [HttpGet("success")]
        public async Task<ActionResult> Success(int orderId)
        {
            var paymentResult = await _paymentService.GetPaymentByOrderId(orderId);
            if (paymentResult?.Data == null)
                return NotFound();

            var service = new SessionService();
            var session = service.Get(paymentResult.Data.SessionId);
            if (session == null)
                throw new BadRequestException("Stripe session not found");

            if (!string.Equals(session.PaymentStatus, PaymentStatus.Paid.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Payment Failed");
            }

            await _paymentService.UpdatePaymentIntentId(orderId, session.Id, session.PaymentIntentId);
            await _paymentService.UpdatePaymentStatus(session.PaymentIntentId, PaymentStatus.Paid);

            await _orderService.UpdateOrderStatus(orderId, OrderStatus.Approved);

            return Ok("Payment Successful!");
        }

        [HttpGet("cancel")]
        public async Task<ActionResult> Cancel()
        {
            return Ok("Payment Cancelled");
        }
    }
}
