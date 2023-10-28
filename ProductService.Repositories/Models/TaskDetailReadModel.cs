
using ProductService.Repositories.Enums;

namespace ProductService.Repositories.Models
{
    public class TaskDetailReadModel
    {
        public int Id { get; set; }
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
