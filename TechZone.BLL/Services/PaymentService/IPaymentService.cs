using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechZone.BLL.DTOs.PaymentDTOs;
using TechZone.BLL.Wrappers;
using TechZone.DAL.Enums.Payment;
using TechZone.DAL.Models;

namespace TechZone.BLL.Services.PaymentService
{
    public interface IPaymentService
    {
        public Task<string> CreateCheckoutSession(List<CheckoutProductDTO> checkoutProductDTO, int orderId);
        public Task AddPayment(PaymentAddDTO paymentAddDTO);

        //When implement webhook
        public Task<Result<PaymentReadDTO>> GetPaymentByIntentId(string paymentIntentId);
        public Task<Result<PaymentReadDTO>> GetPaymentBySessionId(string sessionId);
        public Task UpdatePaymentStatus(string paymentIntentId, PaymentStatus newStatus);
    }
}
