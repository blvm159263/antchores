using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.Repositories.Entities
{
    public class Tasker
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

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
        [Required]
        public int AccountId { get; set; }

        public Account Account { get; set; }
    }
}