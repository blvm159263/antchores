using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductService.Repositories.Entities
{
    public class Tasker{

        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int ExternalId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }
        [Required]
        public string Identification { get; set; }

        [Required]
        public bool Status { get; set; }

        public ICollection<TaskerCert> TaskerCerts { get; set; } = new List<TaskerCert>();

        public ICollection<Contact> Contacts { get; set; } = new List<Contact>();

    }
}
