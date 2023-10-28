using ProductService.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Repositories.Models
{
    public class TaskDetailCreateModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; }
        public DurationUnit Unit { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public int CategoryId { get; set; }
    }
}
