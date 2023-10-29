using Microsoft.EntityFrameworkCore;
using ProductService.Repositories.Data;
using ProductService.Repositories.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Repositories.Repositories.Impl
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly AppDbContext _context;

        public OrderDetailRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<OrderDetail> GetByOrderId(int orderId)
        {
            return _context.OrderDetails
                .Include(x => x.Order)
                .Include(x => x.TaskDetail)
                .Where(od => od.OrderId == orderId);
        }
    }
}
