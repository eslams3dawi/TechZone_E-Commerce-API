using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechZone.DAL.Models;
using TechZone.DAL.Repository.GenericRepo;

namespace TechZone.DAL.Repository.PaymentRepo
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        public Task<Payment?> GetPaymentByIntentId(string paymentIntentId);
        public Task<Payment?> GetPaymentBySessionId(string sessionId);
        public Task UpdatePaymentStatus(Payment payment);
        public Task<Payment?> GetPayment(int orderHeaderId);
        public Task<Payment?> GetPaymentByOrderId(int orderId);
    }
}
