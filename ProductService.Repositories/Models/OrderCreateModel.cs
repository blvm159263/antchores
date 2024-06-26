using System;
using System.ComponentModel.DataAnnotations;
using ProductService.Repositories.Enums;

namespace ProductService.Repositories.Models
{
    public class OrderCreateModel
    {
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime WorkingAt { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public PaymentStatus PaymentStatus { get; set; }
        [Required]
        public decimal Total { get; set; }
        [Required]
        public bool Status { get; set; }
    }
}