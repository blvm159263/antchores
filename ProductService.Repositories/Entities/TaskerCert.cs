using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProductService.Repositories.Entities;
namespace ProductService.Repositories.Entities
{
    public class TaskerCert
    {
        [Key]
        [Column(Order = 0)]
        public int TaskerId { get; set; }
        [Key]
        [Column(Order = 1)]
        public int CategoryId { get; set; }
        [Required]
        public DateTime CreateAt { get; set; }
        [Required]
        public bool Status { get; set; }

        public Tasker Tasker { get; set; }

        public Category Category { get; set; }
    }
}