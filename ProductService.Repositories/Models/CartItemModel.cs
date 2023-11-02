using ProductService.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Repositories.Models
{
    public class CartItemModel
    {
        public int TaskDetailId { get; set; }
        public decimal Price { get; set; }
        public QuantityUnit Unit { get; set; }
        public int Quantity { get; set; }
    }
}
