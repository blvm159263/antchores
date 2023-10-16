using System;
using ProductService.Enums;

namespace ProductService.Models
{
    public class OrderReadModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime WorkingAt { get; set; }
        public DateTime StartTime { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public decimal Total { get; set; }
        public bool Status { get; set; }
        public int CustomerId { get; set; }
    }
}