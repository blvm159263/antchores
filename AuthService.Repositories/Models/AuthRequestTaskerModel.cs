using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Repositories.Models
{
    public class AuthRequestTaskerModel
    {
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Identification { get; set; }
        public bool Status { get; set; } = true;
    }
}
