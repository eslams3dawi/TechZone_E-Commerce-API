using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TechZone.DAL.Models;
using TechZone.DAL.Repository.GenericRepo;

namespace TechZone.DAL.Repository.OrderRepo
{
    public interface IOrderRepository : IGenericRepository<OrderHeader>
    {
        public Task<IEnumerable<OrderDetail>> GetOrderDetails(int orderHeaderId);
        public Task<int> AddOrderAndReturnId(OrderHeader orderHeader);
    }
}
