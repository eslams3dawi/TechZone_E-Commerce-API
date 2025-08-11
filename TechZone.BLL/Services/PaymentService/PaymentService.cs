using AutoMapper;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechZone.BLL.DTOs.PaymentDTOs;
using TechZone.BLL.Wrappers;
using TechZone.DAL.Enums.Payment;
using TechZone.DAL.Models;
using TechZone.DAL.Repository.PaymentRepo;
using Stripe.Checkout;
using static System.Runtime.InteropServices.JavaScript.JSType;
using TechZone.DAL.Repository.OrderRepo;
using Stripe.Climate;
using Microsoft.EntityFrameworkCore;
using TechZone.DAL.Enums.Order;
using TechZone.API.Middleware.CustomExceptions;
using TechZone.BLL.Services.OrderService;

namespace TechZone.BLL.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderService _orderService;

        public PaymentService(IPaymentRepository paymentRepository, IMapper mapper, IOrderRepository orderRepository, IOrderService orderService)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _orderRepository = orderRepository;
            _orderService = orderService;
        }
        public async Task<Result<string>> CreateCheckoutSession(/*List<CheckoutProductDTO> checkoutProductDTOs,*/ int orderId)
        {
            var orderHeader = await _orderRepository.GetById(orderId);
            if(orderHeader == null || orderHeader.OrderStatus == OrderStatus.Approved)
            {
                return Result<string>.Failure("Invalid or already paid order", null, ActionCode.BadRequest);
            }
            var orderDetails = await _orderService.GetOrderDetails(orderId);

            var options = new SessionCreateOptions
            {
                SuccessUrl = $"https://localhost:7188/api/Payments/success?orderId={orderId}",
                CancelUrl = "https://localhost:7188/api/Payments/cancel",
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };
            
            foreach(var item in orderDetails.Data)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.ProductName
                        }
                    },
                    Quantity = item.Count,
                };
                options.LineItems.Add(sessionLineItem);
            }

            var service = new SessionService();
            var session = await service.CreateAsync(options);//exception here 

            var existingPayment = await _paymentRepository.GetPayment(orderId);

            if(existingPayment == null)
            {
                var payment = new Payment()
                {
                    Amount = orderDetails.Data.Sum(p => p.Price * p.Count),
                    Method = "card",
                    PaymentDate = DateTime.UtcNow,
                    SessionId = session.Id,
                    Status = PaymentStatus.Pending,
                    PaymentIntentId = session.PaymentIntentId,
                    OrderHeaderId = orderId
                };
                await _paymentRepository.Add(payment);
            }
            else
                await UpdatePaymentIntentId(orderId, session.Id, session.PaymentIntentId);//why paymentIntentId is null

            return Result<string>.Success(session.Url);
        }
        public async Task UpdatePaymentIntentId(int orderId, string sessionId, string paymentIntentId)
        {
            var payment = await _paymentRepository.GetPaymentByOrderId(orderId);

            if (payment == null)
                throw new BadRequestException($"Payment not found for order ID: {orderId}");

            payment.SessionId = sessionId;
            payment.PaymentIntentId = paymentIntentId;

            await _paymentRepository.Update(payment);
        }

        public async Task<Result<Payment>> GetPaymentByOrderId(int orderId)
        {
            var payment = await _paymentRepository.GetPaymentByOrderId(orderId);
            if (payment == null)
                return Result<Payment>.Failure("Payment not found", null, ActionCode.NotFound);

            return Result<Payment>.Success(payment);
        }

        public async Task AddPayment(PaymentAddDTO paymentAddDTO)
        {
            var paymentModel = _mapper.Map<Payment>(paymentAddDTO);

            await _paymentRepository.Add(paymentModel);
        }

        public async Task<Result<PaymentReadDTO>> GetPaymentByIntentId(string paymentIntentId)
        {
            var paymentModel = await _paymentRepository.GetPaymentByIntentId(paymentIntentId);
            if (paymentModel == null)
                return Result<PaymentReadDTO>.Failure($"Payment with intent Id: {paymentIntentId} not found", null, ActionCode.NotFound);
            
            var paymentDTO = _mapper.Map<PaymentReadDTO>(paymentModel);
            return Result<PaymentReadDTO>.Success(paymentDTO);
        }

        public async Task<Result<PaymentReadDTO>> GetPaymentBySessionId(string sessionId)
        {
            var paymentModel = await _paymentRepository.GetPaymentBySessionId(sessionId);

            var paymentDTO = _mapper.Map<PaymentReadDTO>(paymentModel);
            return Result<PaymentReadDTO>.Success(paymentDTO);
        }

        public async Task UpdatePaymentStatus(string paymentIntentId, PaymentStatus newStatus)
        {
            var paymentModel = await _paymentRepository.GetPaymentByIntentId(paymentIntentId);

            if (paymentModel.Status == newStatus)
                return;

            paymentModel.Status = newStatus;
            await _paymentRepository.Update(paymentModel);
        }
    }
}
