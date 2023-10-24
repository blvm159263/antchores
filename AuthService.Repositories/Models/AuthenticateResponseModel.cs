using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Repositories.Models
{
    public class AuthenticateResponseModel
    {
        public AccountModel Account { get; set; }
        public string Token { get; set; }
    }
}
