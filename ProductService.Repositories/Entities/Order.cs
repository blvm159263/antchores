using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProductService.Repositories.Enums;

namespace ProductService.Repositories.Entities
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
        [Column(TypeName = "nvarchar(50)")]
        public OrderEnum State { get; set; } = OrderEnum.Pending;

        [Required]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        public ICollection<Contract> Contracts { get; set; } = new List<Contract>();

        public DateTime GetEndTime()
        {
            // Perform some logic to obtain the EndTime
            DateTime endTime = this.StartTime;
            foreach (OrderDetail detail in OrderDetails)
            {
                var unit = detail.TaskDetail.Unit;
                endTime = endTime.AddMinutes(detail.TaskDetail.Duration);
            }
            return endTime; // Adjust this according to your data model
        }
    }
}