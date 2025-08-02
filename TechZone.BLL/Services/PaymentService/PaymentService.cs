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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TechZone.BLL.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;

        public PaymentService(IPaymentRepository paymentRepository, IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
        }
        public async Task AddPayment(PaymentAddDTO paymentAddDTO)
        {
            var paymentModel = _mapper.Map<Payment>(paymentAddDTO);

            await _paymentRepository.Add(paymentModel);
        }

        public async Task<string> CreateCheckoutSession(List<CheckoutProductDTO> checkoutProductDTOs, int orderId)
        {
            var LineItems = checkoutProductDTOs.Select(product => new Stripe.Checkout.SessionLineItemOptions
            {
                PriceData = new Stripe.Checkout.SessionLineItemPriceDataOptions
                {
                    UnitAmount = product.Price,
                    Currency = "usd",

                    ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
                    {
                        Name = product.Name
                    }
                },
                Quantity = product.Quantity,
            }).ToList();

            var options = new Stripe.Checkout.SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = LineItems,
                Mode = "payment",
                SuccessUrl = "https://localhost:7188/api/Payments/success",
                CancelUrl = "https://localhost:7188/api/Payments/cancel"
            };

            var service = new Stripe.Checkout.SessionService();
            var session = await service.CreateAsync(options);

            var existingPayment = await _paymentRepository.GetPayment(orderId);

            if(existingPayment == null)
            {
                var payment = new Payment()
                {
                    Amount = checkoutProductDTOs.Sum(p => (decimal)p.Price * p.Quantity),
                    Method = "card",
                    PaymentDate = DateTime.UtcNow,
                    SessionId = session.Id,
                    Status = PaymentStatus.Pending,
                    PaymentIntentId = session.PaymentIntentId,
                    OrderHeaderId = orderId
                };
                await _paymentRepository.Add(payment);
            }

            return session.Url;
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
