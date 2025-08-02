using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechZone.DAL.Database;
using TechZone.DAL.Models;
using TechZone.DAL.Repository.GenericRepo;

namespace TechZone.DAL.Repository.PaymentRepo
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        private readonly TechZoneContext _context;

        public PaymentRepository(TechZoneContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Payment?> GetPayment(int orderHeaderId)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.OrderHeaderId == orderHeaderId);
        }

        public async Task<Payment?> GetPaymentByIntentId(string paymentIntentId)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.PaymentIntentId == paymentIntentId);
        }

        public async Task<Payment?> GetPaymentBySessionId(string sessionId)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.SessionId == sessionId);
        }

        public async Task UpdatePaymentStatus(Payment payment)
        {
            await _context.SaveChangesAsync();
        }
    }
}
