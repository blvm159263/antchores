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
    }
}