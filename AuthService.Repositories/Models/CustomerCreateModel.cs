using System.ComponentModel.DataAnnotations;

namespace AuthService.Repositories.Models
{
    public class CustomerCreateModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public bool Status { get; set; }
        
    }
}