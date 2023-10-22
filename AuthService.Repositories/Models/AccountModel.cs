using AuthService.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Repositories.Models
{
    public class AccountModel
    {
        public string PhoneNumber { get; set; }
        public Role Role { get; set; }
        public int UserId { get; set; }
    }
}
