using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Repositories.Models
{
    public class OrderDetailReadModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int TaskDetailId { get; set; }
        public string TaskDetailName { get; set; }
        public int Quantity { get; set; }
    }
}
