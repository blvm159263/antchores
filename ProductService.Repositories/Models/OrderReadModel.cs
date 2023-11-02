using System;
using ProductService.Repositories.Enums;

namespace ProductService.Repositories.Models
{
    public class OrderReadModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime WorkingAt { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Address { get; set; }
        public string CategoryName { get; set; }
        public OrderEnum State { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public decimal Total { get; set; }
        public bool Status { get; set; }
        public int CustomerId { get; set; }
    }
}