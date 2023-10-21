using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProductService.BusinessObjects.Enums;

namespace ProductService.BusinessObjects.Entities
{
    public class Order
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime WorkingAt { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public PaymentStatus PaymentStatus { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Total { get; set; }
        [Required]
        public bool Status { get; set; }

        [Required]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}