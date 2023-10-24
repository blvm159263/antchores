using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ProductService.Repositories.Entities
{
    public class Contact
    {
        [Key]
        [Column(Order = 0)]
        public int OrderId { get; set; }
        [Key]
        [Column(Order = 1)]
        public int TaskerId { get; set; }

        public DateTime CreateAt { get; set; }

        public bool Status { get; set; }

        public Tasker Tasker { get; set; }

        public Order Order { get; set; }

    }
}