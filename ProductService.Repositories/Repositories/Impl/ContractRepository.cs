using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProductService.Repositories.Data;
using ProductService.Repositories.Entities;

namespace ProductService.Repositories.Repositories.Impl
{
    public class ContractRepository : IContractRepository
    {

        private readonly AppDbContext _context;

        public ContractRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool CreateContact(Contract con)
        {
            if (con == null)
            {
                throw new ArgumentNullException(nameof(con));
            }
            _context.Contracts.Add(con);
            return _context.SaveChanges() > 0;
        }

        public IEnumerable<Contract> GetContractsByTaskerId(int taskerId)
        {
            return _context.Contracts
                    .Include(x => x.Tasker)
                    .Include(x => x.Order)
                        .ThenInclude(c => c.OrderDetails)
                        .ThenInclude(od => od.TaskDetail)
                        .ThenInclude(td => td.Category)
                    .Include(o => o.Order.Customer)
                    .Where(x => x.TaskerId == taskerId);
        }

        public Contract GetContractsByTaskerIdAndOrderId(int taskerId, int orderId)
        {
             return _context.Contracts
                    .Include(x => x.Tasker)
                    .Include(x => x.Order)
                        .ThenInclude(c => c.OrderDetails)
                        .ThenInclude(od => od.TaskDetail)
                        .ThenInclude(td => td.Category)
                    .Include(o => o.Order.Customer)
                    .FirstOrDefault(x => x.TaskerId == taskerId && x.OrderId == orderId);
        }
    }
}