using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductService.BusinessObjects.Entities
{
    public class Category
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool Status { get; set; }
        public ICollection<TaskerCert> TaskerCerts { get; set; } = new List<TaskerCert>();
        public ICollection<TaskDetail> TaskDetails { get; set; } = new List<TaskDetail>();

    }
}