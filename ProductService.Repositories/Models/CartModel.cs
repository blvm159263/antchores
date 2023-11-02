using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Repositories.Models
{
    public class CartModel
    {
        public int CustomerId { get; set; }
        public DateTime WorkingAt { get; set; }
        public DateTime StartTime { get; set; }
        public List<CartItemModel> CartItems { get; set; } = new List<CartItemModel>();
    }
}
