using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TechZone.DAL.Database;
using TechZone.DAL.Models;
using TechZone.DAL.Repository.GenericRepo;

namespace TechZone.DAL.Repository.OrderRepo
{
    public class OrderRepository : GenericRepository<OrderHeader>, IOrderRepository
    {
        private readonly TechZoneContext _context;

        public OrderRepository(TechZoneContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> AddOrderAndReturnId(OrderHeader orderHeader)
        {
            await _context.OrdersHeaders.AddAsync(orderHeader);
            await _context.SaveChangesAsync();

            return orderHeader.Id;
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetails(int orderHeaderId)
        {
            return await _context.OrderDetails
                .Where(o => o.OrderHeaderId == orderHeaderId)
                .Include(o => o.Product)
                .ToListAsync();
        }
    }
}
