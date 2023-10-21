using System.ComponentModel.DataAnnotations;

namespace AuthService.BusinessObjects.Models
{
    public class TaskerCreateModel
    {
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
        
    }
}